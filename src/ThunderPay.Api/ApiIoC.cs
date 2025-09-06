using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ThunderPay.Api.Controllers;

namespace ThunderPay.Api;
public static class ApiIoC
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ThunderPay API",
            });
        });

        services.AddControllers().AddApplicationPart(typeof(TransactionsController).Assembly);
    }

    public static void ConfigurePipeline(this WebApplication app)
    {
        app.MapControllers();
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
