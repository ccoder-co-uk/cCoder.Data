using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace cCoder.Data;

public interface ICoreContextFactory
{
    CoreDataContext CreateCoreContext();
}

public class CoreContextFactory : ICoreContextFactory, IDesignTimeDbContextFactory<CoreDataContext>
{
    private IServiceProvider serviceProvider;

    public CoreContextFactory()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        string connection = configuration.GetConnectionString("Core");

        ServiceCollection services = [];
        services.AddLogging();
        services.AddCoreData(connection);

        serviceProvider = services.BuildServiceProvider();
    }

    public CoreContextFactory(IServiceProvider serviceProvider) =>
        this.serviceProvider = serviceProvider;

    public CoreDataContext CreateCoreContext()
    {
        if (serviceProvider is null)
            return CreateDbContext([]);

        return new(
            serviceProvider.GetRequiredService<ICoreAuthInfo>(),
            serviceProvider.GetRequiredService<Config>(),
            serviceProvider.GetRequiredService<ILogger<CoreDataContext>>());
    }

    public CoreDataContext CreateDbContext(string[] args) =>
         CreateCoreContext();
}
