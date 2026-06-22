using cCoder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace cCoder.PostgresSQL;

/// <summary>
/// Design-time factory for CoreDataContext used by EF Core CLI tools.
/// Allows 'dotnet ef' commands to create and configure the DbContext without dependency injection.
/// </summary>
public class CoreDataContextDesignTimeFactory : IDesignTimeDbContextFactory<CoreDataContext>
{
    public CoreDataContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("CCODER_CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "CCODER_CONNECTION_STRING environment variable is required for migrations.");

        var optionsBuilder = new DbContextOptionsBuilder<CoreDataContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(CoreDataContextDesignTimeFactory).Assembly.GetName().Name));

        // Create minimal instances for design-time use
        var authInfo = new CoreAuthInfo(); // Uses default SSOUserId (empty string)
        var config = new Config
        {
            ConnectionStrings = new Dictionary<string, string> { ["Core"] = connectionString },
            Settings = new Dictionary<string, string>(),
            Services = new Dictionary<string, string>(),
        };
        var logger = LoggerFactory.Create(builder => { }).CreateLogger<CoreDataContext>();

        return new CoreDataContext(optionsBuilder.Options, authInfo, config, logger);
    }
}
