using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ThunderPay.Api;
using ThunderPay.Application;
using ThunderPay.Database;

namespace ThunderPay.Server;
public class Program
{
    private static void Main(string[] args)
    {
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddProblemDetails();

        builder.Services.ConfigureOpenTelemetryTracerProvider((sp, tp) =>
            tp.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ThunderPay.Server")));

        builder.Services.AddOpenTelemetry()
            .UseAzureMonitor();

        DatabaseIoC.RegisterDatabaseServices(builder.Services);
        ApplicationIoC.RegisterServices(builder.Services);
        ApiIoC.RegisterServices(builder.Services);

        var app = builder.Build();
        app.MapGet("/ping", () => "ok");

        app.UseExceptionHandler();

        DatabaseIoC.Initialize(app.Services);
        ApiIoC.ConfigurePipeline(app);

        app.Run();
    }
}