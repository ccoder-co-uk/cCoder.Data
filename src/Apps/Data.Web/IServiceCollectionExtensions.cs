// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Dependencies;
using Data.Web.Services.Foundations;
using Data.Web.Services.Orchestrations;
using Data.Web.Services.Processings;

namespace Data.Web;

internal static class IServiceCollectionExtensions
{
    internal static void AddDataWeb(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(setupAction:options =>
        {
            options.SwaggerDoc(name:"Data", info:new()
            {
                Title = "Data Tooling API",
                Version = "v1"
            });
        });

        services.AddTransient<IDataSetBroker, DataSetBroker>();
        services.AddTransient<IDataSetService, DataSetService>();
        services.AddTransient<IDataSetProcessingService, DataSetProcessingService>();
        services.AddTransient<IDataSetOrchestrationService, DataSetOrchestrationService>();
    }
}