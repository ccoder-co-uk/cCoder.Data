// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;
using cCoder.Data.Exposures;
using cCoder.Data.Services.Foundations;
using cCoder.Eventing.Models;
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
        services.AddCoreDataAccess(connectionString:connectionString);
        services.AddCoreAuthInfo();
    }

    public static void AddCoreDataAccess(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.TryAddSingleton(instance:new Config
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

        if (!services.Any(predicate:serviceDescriptor => serviceDescriptor.ServiceType == typeof(IDbContextFactory<CoreDataContext>)))
            services.AddDbContextFactory<CoreDataContext>(lifetime: ServiceLifetime.Scoped);

    }

    public static void AddCoreAuthInfo(this IServiceCollection services)
    {
        services.Replace(descriptor:ServiceDescriptor.Transient<ICoreAuthInfo>(ctx => new CoreAuthInfo
        {
            SSOUserId = ResolveSsoUserId(ctx),
        }));
    }

    private static string ResolveSsoUserId(IServiceProvider serviceProvider)
    {
        string eventUserId = serviceProvider.GetService<IEventAuthInfo>()?.SSOUserId;

        if (!string.IsNullOrWhiteSpace(value:eventUserId))
            return eventUserId;

        string ssoUserId;

        try
        {
            Type authInfoType = Type.GetType(
typeName:                "cCoder.Security.Objects.ISSOAuthInfo, cCoder.Security.Data",
                throwOnError: false);

            object authInfo = authInfoType is null
                ? null
                : serviceProvider.GetService(serviceType:authInfoType);

            ssoUserId = authInfo?.GetType().GetProperty(name:"SSOUserId")?.GetValue(obj:authInfo)?.ToString();
        }
        catch
        {
            ssoUserId = "Guest";
        }

        return string.IsNullOrWhiteSpace(value:ssoUserId)
            ? "Guest"
            : ssoUserId;
    }
}

public class CoreAuthInfo : ICoreAuthInfo
{
    public string SSOUserId { get; internal set; } = string.Empty;
}