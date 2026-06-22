using cCoder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cCoder.PostgresSQL;

public static class IServiceCollectionExtensions
{
    public static void AddCoreDataPostgresSQL(this IServiceCollection services, string connectionString)
    {
        services.AddCoreData(connectionString);
        services.AddCoreDataAccessPostgresSQL(connectionString);
    }

    public static void AddCoreDataAccessPostgresSQL(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<CoreDataContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(IServiceCollectionExtensions).Assembly.GetName().Name)),
            lifetime: ServiceLifetime.Scoped);
    }
}
