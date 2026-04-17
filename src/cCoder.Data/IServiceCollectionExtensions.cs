using cCoder.Data.Brokers.Caching;
using cCoder.Data.Exposures;
using cCoder.Data.Services.Foundations;
using EventLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace cCoder.Data;

public static class IServiceCollectionExtensions
{
    public static void AddCoreData(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddCoreDataAccess(connectionString);
        services.AddCoreAuthInfo();
    }

    public static void AddCoreDataAccess(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.TryAddSingleton(new Config
        {
            ConnectionStrings = new Dictionary<string, string>
            {
                ["Core"] = connectionString,
            },
            Settings = new Dictionary<string, string>(),
            Services = new Dictionary<string, string>(),
        });

        services.TryAddScoped<CoreDataContext>();
        services.TryAddScoped<ICoreContextFactory, CoreContextFactory>();
        services.TryAddSingleton<IMetadataTypeCacheBroker, MetadataTypeCacheBroker>();
        services.TryAddSingleton<IMetadataTypeCacheService, MetadataTypeCacheService>();
        services.TryAddSingleton<IMetadataTypeCache, MetadataTypeCache>();

        if (!services.Any(serviceDescriptor => serviceDescriptor.ServiceType == typeof(IDbContextFactory<CoreDataContext>)))
            services.AddDbContextFactory<CoreDataContext>(lifetime: ServiceLifetime.Scoped);

    }

    public static void AddCoreAuthInfo(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<ICoreAuthInfo>(ctx => new CoreAuthInfo
        {
            SSOUserId = ResolveSsoUserId(ctx),
        }));
    }

    private static string ResolveSsoUserId(IServiceProvider serviceProvider)
    {
        string eventUserId = serviceProvider.GetService<IEventAuthInfo>()?.SSOUserId;

        if (!string.IsNullOrWhiteSpace(eventUserId))
            return eventUserId;

        string ssoUserId;

        try
        {
            object authInfo =
                serviceProvider.GetService(Type.GetType("cCoder.Security.Objects.ISSOAuthInfo, cCoder.Security.Data"))
                ?? serviceProvider.GetService(Type.GetType("cCoder.Security.Objects.ISSOAuthInfo, cCoder.Security.Objects"));

            ssoUserId = authInfo?.GetType().GetProperty("SSOUserId")?.GetValue(authInfo)?.ToString();
        }
        catch
        {
            ssoUserId = "Guest";
        }

        return string.IsNullOrWhiteSpace(ssoUserId)
            ? "Guest"
            : ssoUserId;
    }
}

public class CoreAuthInfo : ICoreAuthInfo
{
    public string SSOUserId { get; internal set; } = string.Empty;
}




