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

namespace cCoder.Data.Services.Processings;

internal sealed partial class ServiceCollectionProcessingService
    : IServiceCollectionProcessingService
{
    public void AddCoreData(
        IServiceCollection services,
        string connectionString) =>
        TryCatch(operation: () =>
        {
            Validate(inputs: [services, connectionString]);

            AddCoreDataAccessInternal(
                services: services,
                connectionString: connectionString);

            AddCoreAuthInfoInternal(services: services);
        });

    public void AddCoreDataAccess(
        IServiceCollection services,
        string connectionString) =>
        TryCatch(operation: () =>
        {
            Validate(inputs: [services, connectionString]);

            AddCoreDataAccessInternal(
                services: services,
                connectionString: connectionString);
        });

    public void AddCoreAuthInfo(IServiceCollection services) =>
        TryCatch(operation: () =>
        {
            Validate(inputs: services);
            AddCoreAuthInfoInternal(services: services);
        });

    private static void AddCoreDataAccessInternal(
        IServiceCollection services,
        string connectionString)
    {
        Config configuration = new()
        {
            ConnectionStrings = new Dictionary<string, string>
            {
                ["Core"] = connectionString,
            },
            Settings = new Dictionary<string, string>(),
            Services = new Dictionary<string, string>(),
        };

        services.TryAddSingleton(instance: configuration);
        services.TryAddScoped<CoreDataContext>();
        services.TryAddScoped<ICoreContextFactory, CoreContextFactory>();
        services.TryAddSingleton<IMetadataTypeCacheBroker, MetadataTypeCacheBroker>();
        services.TryAddSingleton<IMetadataTypeCacheService, MetadataTypeCacheService>();
        services.TryAddSingleton<IMetadataTypeCache, MetadataTypeCache>();

        bool factoryIsRegistered = services.Any(
            predicate: serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(IDbContextFactory<CoreDataContext>));

        if (!factoryIsRegistered)
        {
            services.AddDbContextFactory<CoreDataContext>(
                lifetime: ServiceLifetime.Scoped);
        }
    }

    private static void AddCoreAuthInfoInternal(IServiceCollection services) =>
        services.Replace(
            descriptor: ServiceDescriptor.Transient<ICoreAuthInfo>(
                implementationFactory: serviceProvider =>
                    new CoreAuthInfo
                    {
                        SSOUserId = ResolveSsoUserId(
                            serviceProvider: serviceProvider),
                    }));

    private static string ResolveSsoUserId(IServiceProvider serviceProvider)
    {
        string eventUserId = serviceProvider
            .GetService<IEventAuthInfo>()
            ?.SSOUserId;

        if (!string.IsNullOrWhiteSpace(value: eventUserId))
        {
            return eventUserId;
        }

        string ssoUserId;

        try
        {
            Type authInfoType = Type.GetType(
                typeName: "cCoder.Security.Objects.ISSOAuthInfo, cCoder.Security.Data",
                throwOnError: false);

            object authInfo = authInfoType is null
                ? null
                : serviceProvider.GetService(serviceType: authInfoType);

            ssoUserId = authInfo
                ?.GetType()
                .GetProperty(name: "SSOUserId")
                ?.GetValue(obj: authInfo)
                ?.ToString();
        }
        catch
        {
            ssoUserId = "Guest";
        }

        return string.IsNullOrWhiteSpace(value: ssoUserId)
            ? "Guest"
            : ssoUserId;
    }
}