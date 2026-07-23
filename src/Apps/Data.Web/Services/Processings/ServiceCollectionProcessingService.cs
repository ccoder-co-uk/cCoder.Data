// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Dependencies;
using Data.Web.Services.Foundations;

namespace Data.Web.Services.Processings;

internal sealed partial class ServiceCollectionProcessingService
    : IServiceCollectionProcessingService
{
    public void AddDataWeb(IServiceCollection services) =>
        TryCatch(operation: () =>
        {
            Validate(inputs: services);
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(setupAction: options =>
            {
                options.SwaggerDoc(
                    name: "Data",
                    info: new()
                    {
                        Title = "Data Tooling API",
                        Version = "v1",
                    });
            });

            services.AddTransient<IDataSetBroker, DataSetBroker>();
            services.AddTransient<IDataEntitySetService, DataEntitySetService>();
            services.AddTransient<IDataRowService, DataRowService>();
        });
}