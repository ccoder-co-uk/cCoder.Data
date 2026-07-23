// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using Microsoft.AspNetCore.Diagnostics;

namespace Data.Web;

public class Program
{
    private static ILogger log = null!;

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args:args);

        builder.Services.AddDataWeb(configuration: builder.Configuration);

        WebApplication app = builder.Build();
        log = app.Services.GetRequiredService<ILogger<Program>>();

        app.UseHttpsRedirection();
        app.UseSession();
        app.UseStaticFiles();

        app.UseSwagger()
            .UseSwaggerUI(setupAction:options =>
            {
                options.SwaggerEndpoint(url:"/swagger/Data/swagger.json", name:"Data Tooling API");
                options.SwaggerEndpoint(url:"/swagger/v1/swagger.json", name:"Security API");
            });

        app.MapGet(pattern:"/Health", handler:() => Results.Text(content:"OK"));
        app.MapGet(pattern:"/", handler:() => Results.Redirect(url:"/tools/index.html"));
        app.UseRouting();
        app.MapControllers();

        app.UseExceptionHandler(configure:errorApplication =>
        {
            errorApplication.Run(handler:HandleUnhandledException);
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
        {
            return;
        }

        log.LogError(
            exception: exception,
            message: "{Message}\n{StackTrace}",
            args: [exception.Message, exception.StackTrace]);

        await context.Response.WriteAsync(
            text: "{ \"error\": \"" + exception.Message.Replace(oldValue: "\"", newValue: "'") + "\" }");
    }
}