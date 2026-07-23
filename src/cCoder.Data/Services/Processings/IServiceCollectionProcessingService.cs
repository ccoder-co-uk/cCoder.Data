// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace cCoder.Data.Services.Processings;

internal interface IServiceCollectionProcessingService
{
    void AddCoreData(
        IServiceCollection services,
        string connectionString);

    void AddCoreDataAccess(
        IServiceCollection services,
        string connectionString);

    void AddCoreAuthInfo(IServiceCollection services);
}