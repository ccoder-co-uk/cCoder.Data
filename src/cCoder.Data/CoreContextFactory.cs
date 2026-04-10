using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace cCoder.Data;

public interface ICoreContextFactory
{
    CoreDataContext CreateCoreContext();
}

public class CoreContextFactory(IServiceProvider serviceProvider) : ICoreContextFactory
{
    public CoreDataContext CreateCoreContext() =>
        new(
            serviceProvider.GetRequiredService<ICoreAuthInfo>(),
            serviceProvider.GetRequiredService<Config>(),
            serviceProvider.GetRequiredService<ILogger<CoreDataContext>>());
}




