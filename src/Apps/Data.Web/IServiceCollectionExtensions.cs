// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using cCoder.Security;
using Data.Web.Brokers;
using Data.Web.Dependencies;
using Data.Web.Models;
using Data.Web.Services.Foundations;

namespace Data.Web;

public static class IServiceCollectionExtensions
{
    public static void AddDataWeb(
        this IServiceCollection services,
        IConfiguration applicationConfiguration,
        Action<DataWebConfiguration> configure = null)
    {
        DataWebConfiguration configuration = new();
        applicationConfiguration.Bind(configuration);
        configure?.Invoke(configuration);

        services.AddBrokers();
        services.AddFoundations();
        services.AddExposures();

        cCoder.Data.IServiceCollectionExtensions.AddData(
            services,
            configuration.Data);

        services.AddSecurityWeb(configuration.Security);
    }

    private static void AddBrokers(this IServiceCollection services) =>
        services.AddTransient<IDataSetBroker, DataSetBroker>();

    private static void AddFoundations(this IServiceCollection services)
    {
        services.AddTransient<IDataEntitySetService, DataEntitySetService>();
        services.AddTransient<IDataRowService, DataRowService>();
    }

    private static void AddExposures(this IServiceCollection services)
    {
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
    }
}