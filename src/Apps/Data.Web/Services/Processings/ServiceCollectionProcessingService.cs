// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Security;
using cCoder.Security.Data.EF;
using Data.Web.Brokers;
using Data.Web.Dependencies;
using Data.Web.Services.Foundations;

namespace Data.Web.Services.Processings;

internal sealed partial class ServiceCollectionProcessingService
    : IServiceCollectionProcessingService
{
    public void AddDataWeb(
        IServiceCollection services,
        IConfiguration configuration) =>
        TryCatch(operation: () =>
        {
            Validate(inputs: [services, configuration]);

            string coreConnection = configuration.GetConnectionString(name: "Core")
                ?? throw new InvalidOperationException("ConnectionStrings:Core is required.");

            string ssoConnection = configuration.GetConnectionString(name: "SSO")
                ?? throw new InvalidOperationException("ConnectionStrings:SSO is required.");

            cCoder.Data.Config config = new();
            configuration.Bind(instance: config);
            services.AddSingleton(implementationInstance: config);

            services.AddSecurityApi(configAction: (securityServices, securityConfig) =>
            {
                securityConfig.AddMSSQLModelProvider(
                    services: securityServices,
                    connectionString: ssoConnection);

                securityConfig.UseAESHMMACPasswordEncryption(
                    services: securityServices,
                    decryptionKey: configuration.GetSection(key: "Settings")["DecryptionKey"]);
            });

            cCoder.Data.IServiceCollectionExtensions.AddCoreData(
                services: services,
                connectionString: coreConnection);

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
            services.AddTransient<IDataSetDependency, DataSetDependency>();
            services.AddTransient<IDataEntitySetService, DataEntitySetService>();
            services.AddTransient<IDataRowService, DataRowService>();
        });
}