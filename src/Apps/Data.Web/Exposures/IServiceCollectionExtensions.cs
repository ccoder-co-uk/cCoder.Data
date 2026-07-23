// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Services.Processings;

namespace Data.Web;

internal static class IServiceCollectionExtensions
{
    internal static void AddDataWeb(this IServiceCollection services) =>
        new ServiceCollectionProcessingService()
            .AddDataWeb(services: services);
}