using cCoder.Data;
using cCoder.Data.Models.Security;
using cCoder.SQLServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string coreConnectionString = builder.Configuration.GetConnectionString("Core")
    ?? throw new InvalidOperationException("ConnectionStrings:Core is required.");

builder.Services.AddCoreDataSqlServer(coreConnectionString);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { Status = "ok" }));

app.MapPost("/db/migrate", async (ICoreContextFactory coreContextFactory, CancellationToken cancellationToken) =>
{
    await using CoreDataContext context = coreContextFactory.CreateCoreContext();

    int knownMigrationCount = context.Database.GetMigrations().Count();

    if (knownMigrationCount > 0)
    {
        await context.Database.MigrateAsync(cancellationToken);
    }
    else
    {
        await context.Database.EnsureCreatedAsync(cancellationToken);
    }

    int appliedMigrationCount = context.Database.GetAppliedMigrations().Count();

    return Results.Ok(new
    {
        AppliedMigrationCount = appliedMigrationCount,
        MigrationMode = knownMigrationCount > 0 ? "migrate" : "ensure-created",
    });
});

app.MapPost("/db/users/seed", async (SeedUserRequest request, ICoreContextFactory coreContextFactory, CancellationToken cancellationToken) =>
{
    await using CoreDataContext context = coreContextFactory.CreateCoreContext();

    string userId = string.IsNullOrWhiteSpace(request.UserId)
        ? $"e2e-{Guid.NewGuid():N}"
        : request.UserId;

    bool userExists = await context.Users
        .IgnoreQueryFilters()
        .AnyAsync(user => user.Id == userId, cancellationToken);

    if (!userExists)
    {
        context.Users.Add(new User
        {
            Id = userId,
            DefaultCultureId = "en-GB",
            DisplayName = "E2E User",
            Email = $"{userId}@example.test",
            IsActive = true,
        });

        await context.SaveChangesAsync(cancellationToken);
    }

    return Results.Ok(new { UserId = userId });
});

app.MapGet("/db/users/count", async (ICoreContextFactory coreContextFactory, CancellationToken cancellationToken) =>
{
    await using CoreDataContext context = coreContextFactory.CreateCoreContext();

    int userCount = await context.Users
        .IgnoreQueryFilters()
        .CountAsync(cancellationToken);

    return Results.Ok(new { Count = userCount });
});

app.Run();

public sealed record SeedUserRequest(string? UserId);
public partial class Program;
