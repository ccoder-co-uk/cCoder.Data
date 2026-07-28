// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers.Caching;
using cCoder.Data.Exposures;
using cCoder.Data.Extensions;
using cCoder.Data.Models;
using cCoder.Data.Services.Foundations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace cCoder.Data;

public static class IServiceCollectionExtensions
{
    public static void AddData(
        this IServiceCollection services,
        Action<DataConfiguration> configure)
    {
        DataConfiguration configuration = new();
        configure?.Invoke(configuration);
        services.AddData(configuration);
    }

    public static void AddData(
        this IServiceCollection services,
        DataConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        services.AddDependencies(configuration);
        services.AddBrokers();
        services.AddFoundations();
        services.AddExposures();
    }

    private static void AddDependencies(
        this IServiceCollection services,
        DataConfiguration configuration)
    {
        services.TryAddSingleton(instance: configuration);
        services.TryAddScoped<CoreDataContext>();
        services.TryAddScoped<ICoreContextFactory, CoreContextFactory>();

        bool factoryIsRegistered = services.Any(
            predicate: serviceDescriptor =>
                serviceDescriptor.ServiceType ==
                    typeof(IDbContextFactory<CoreDataContext>));

        if (!factoryIsRegistered)
        {
            services.AddDbContextFactory<CoreDataContext>(
                lifetime: ServiceLifetime.Scoped);
        }
    }

    private static void AddBrokers(this IServiceCollection services) =>
        services.TryAddSingleton<
            IMetadataTypeCacheBroker,
            MetadataTypeCacheBroker>();

    private static void AddFoundations(this IServiceCollection services)
    {
        services.TryAddSingleton<
            IMetadataTypeCacheService,
            MetadataTypeCacheService>();

        services.Replace(
            descriptor: ServiceDescriptor.Transient<ICoreAuthInfo>(
                implementationFactory: serviceProvider =>
                    new CoreAuthInfo
                    {
                        SSOUserId =
                            serviceProvider.ResolveSsoUserId(),
                    }));
    }

    private static void AddExposures(this IServiceCollection services) =>
        services.TryAddSingleton<IMetadataTypeCache, MetadataTypeCache>();
}