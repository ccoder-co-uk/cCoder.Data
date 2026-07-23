// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Services.Processings;
using Microsoft.Extensions.DependencyInjection;

namespace cCoder.Data;

public static class IServiceCollectionExtensions
{
    public static void AddCoreData(
        this IServiceCollection services,
        string connectionString) =>
        CreateServiceCollectionProcessingService()
            .AddCoreData(
                services: services,
                connectionString: connectionString);

    public static void AddCoreDataAccess(
        this IServiceCollection services,
        string connectionString) =>
        CreateServiceCollectionProcessingService()
            .AddCoreDataAccess(
                services: services,
                connectionString: connectionString);

    public static void AddCoreAuthInfo(this IServiceCollection services) =>
        CreateServiceCollectionProcessingService()
            .AddCoreAuthInfo(services: services);

    private static ServiceCollectionProcessingService CreateServiceCollectionProcessingService() =>
        new ServiceCollectionProcessingService();
}