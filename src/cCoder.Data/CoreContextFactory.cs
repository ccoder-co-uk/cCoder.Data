using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace cCoder.Data;

public interface ICoreContextFactory
{
    CoreDataContext CreateCoreContext();
}

public class CoreContextFactory(IServiceProvider serviceProvider) : ICoreContextFactory
{
    public CoreDataContext CreateCoreContext()
    {
        IDbContextFactory<CoreDataContext> dbContextFactory =
            serviceProvider.GetService<IDbContextFactory<CoreDataContext>>();

        if (dbContextFactory is not null)
            return dbContextFactory.CreateDbContext();

        return new(
            serviceProvider.GetRequiredService<DbContextOptions<CoreDataContext>>(),
            serviceProvider.GetRequiredService<ICoreAuthInfo>(),
            serviceProvider.GetRequiredService<Config>(),
            serviceProvider.GetRequiredService<ILogger<CoreDataContext>>());
    }
}




