using cCoder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cCoder.SQLServer;

public static class IServiceCollectionExtensions
{
    public static void AddCoreDataSqlServer(this IServiceCollection services, string connectionString)
    {
        services.AddCoreData(connectionString);
        services.AddCoreDataAccessSqlServer(connectionString);
    }

    public static void AddCoreDataAccessSqlServer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<CoreDataContext>(
            optionsBuilder => optionsBuilder.UseSqlServer(
                connectionString,
                sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(IServiceCollectionExtensions).Assembly.GetName().Name)),
            lifetime: ServiceLifetime.Scoped);
    }
}
