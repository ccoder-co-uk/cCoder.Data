// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cCoder.Data.Models;


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

        DataConfiguration dataConfiguration = new();
        configuration
            .GetSection(key: "Data")
            .Bind(instance: dataConfiguration);

        ServiceCollection services = [];
        services.AddLogging();
        services.AddData(configuration: dataConfiguration);

        serviceProvider = services.BuildServiceProvider();
    }

    public CoreContextFactory(IServiceProvider serviceProvider) =>
        this.serviceProvider = serviceProvider;

    public CoreDataContext CreateCoreContext()
    {
        if (serviceProvider is null)
        {
            return CreateDbContext(args:[]);
        }

        return new(
            serviceProvider.GetRequiredService<ICoreAuthInfo>(),
            serviceProvider.GetRequiredService<DataConfiguration>(),
            serviceProvider.GetRequiredService<ILogger<CoreDataContext>>());
    }

    public CoreDataContext CreateDbContext(string[] args) =>
        CreateCoreContext();
}
