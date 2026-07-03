using System.Security;
using cCoder.Security;
using cCoder.Security.Data.EF;
using Microsoft.AspNetCore.Diagnostics;

namespace Data.Web;

public class Program
{
    private static ILogger log = null!;

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string coreConnection = builder.Configuration.GetConnectionString("Core")
            ?? throw new InvalidOperationException("ConnectionStrings:Core is required.");

        string ssoConnection = builder.Configuration.GetConnectionString("SSO")
            ?? throw new InvalidOperationException("ConnectionStrings:SSO is required.");

        cCoder.Data.Config config = new();
        builder.Configuration.Bind(config);
        builder.Services.AddSingleton(config);

        builder.Services.AddSecurityApi((services, securityConfig) =>
        {
            securityConfig.AddMSSQLModelProvider(services, ssoConnection);
            securityConfig.UseAESHMMACPasswordEncryption(
                services,
                builder.Configuration.GetSection("Settings")["DecryptionKey"]);
        });

        cCoder.Data.IServiceCollectionExtensions.AddCoreData(
            builder.Services,
            coreConnection);

        builder.Services.AddDataWeb();

        WebApplication app = builder.Build();
        log = app.Services.GetRequiredService<ILogger<Program>>();

        app.UseHttpsRedirection();
        app.UseSession();
        app.UseStaticFiles();

        app.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Data/swagger.json", "Data Tooling API");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Security API");
            });

        app.MapGet("/Health", () => Results.Text("OK"));
        app.MapGet("/", () => Results.Redirect("/tools/index.html"));
        app.UseRouting();
        app.MapControllers();
        app.UseExceptionHandler(errorApplication =>
        {
            errorApplication.Run(HandleUnhandledException);
        });

        app.Run();
    }

    private static async Task HandleUnhandledException(HttpContext context)
    {
        Exception exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        context.Response.StatusCode =
            exception?.GetType() == typeof(SecurityException) ? 401 : 500;
        context.Response.ContentType = "application/json";

        if (exception is null)
            return;

        log.LogError("{Message}\n{StackTrace}", exception.Message, exception.StackTrace);
        await context.Response.WriteAsync(
            "{ \"error\": \"" + exception.Message.Replace("\"", "'") + "\" }");
    }
}
