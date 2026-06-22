using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace cCoder.SQLServer.E2E;

public sealed class SqlServerDataE2ETests : IClassFixture<SqlServerApiFactory>
{
    private readonly SqlServerApiFactory factory;

    public SqlServerDataE2ETests(SqlServerApiFactory factory) =>
        this.factory = factory;

    [Fact]
    public async Task ShouldMigrateAndQueryUsersViaHttp()
    {
        using HttpClient client = factory.CreateClient();

        HttpResponseMessage migrateResponse = await client.PostAsync("/db/migrate", content: null);
        migrateResponse.EnsureSuccessStatusCode();

        HttpResponseMessage seedResponse = await client.PostAsJsonAsync(
            "/db/users/seed",
            new { UserId = $"sql-{Guid.NewGuid():N}" });

        seedResponse.EnsureSuccessStatusCode();

        UserCountResponse? countResponse = await client.GetFromJsonAsync<UserCountResponse>("/db/users/count");

        Assert.NotNull(countResponse);
        Assert.True(countResponse.Count >= 1);
    }

    private sealed record UserCountResponse(int Count);
}

public sealed class SqlServerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("YourStrong!Passw0rd")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("ConnectionStrings:Core", sqlContainer.GetConnectionString());
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Core"] = sqlContainer.GetConnectionString(),
            });
        });
    }

    public async Task InitializeAsync() =>
        await sqlContainer.StartAsync();

    public new async Task DisposeAsync() =>
        await sqlContainer.DisposeAsync();
}
