using Data.Web.Brokers;
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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("Data", new()
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
