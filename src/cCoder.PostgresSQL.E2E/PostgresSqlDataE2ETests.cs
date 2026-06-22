using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace cCoder.PostgresSQL.E2E;

public sealed class PostgresSqlDataE2ETests : IClassFixture<PostgresSqlApiFactory>
{
    private readonly PostgresSqlApiFactory factory;

    public PostgresSqlDataE2ETests(PostgresSqlApiFactory factory) =>
        this.factory = factory;

    [Fact]
    public async Task ShouldMigrateAndQueryUsersViaHttp()
    {
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage migrateResponse = await client.PostAsync("/db/migrate", content: null);
        migrateResponse.EnsureSuccessStatusCode();

        HttpResponseMessage seedResponse = await client.PostAsJsonAsync(
            "/db/users/seed",
            new { UserId = $"pg-{Guid.NewGuid():N}" });

        seedResponse.EnsureSuccessStatusCode();

        UserCountResponse? countResponse = await client.GetFromJsonAsync<UserCountResponse>("/db/users/count");

        Assert.NotNull(countResponse);
        Assert.True(countResponse.Count >= 1);
    }

    private sealed record UserCountResponse(int Count);
}

public sealed class PostgresSqlApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithDatabase("ccoder_postgresql_e2e")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("ConnectionStrings:Core", postgresContainer.GetConnectionString());
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Core"] = postgresContainer.GetConnectionString(),
            });
        });
    }

    public async Task InitializeAsync() =>
        await postgresContainer.StartAsync();

    public new async Task DisposeAsync() =>
        await postgresContainer.DisposeAsync();
}
