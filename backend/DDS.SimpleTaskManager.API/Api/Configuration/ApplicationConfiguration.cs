using System.Text.Json.Serialization;

using DDS.SimpleTaskManager.API.Api.Endpoints;
using DDS.SimpleTaskManager.API.Application;
using DDS.SimpleTaskManager.API.Infrastructure;
using DDS.SimpleTaskManager.Core.Middlewares;
using DDS.SimpleTaskManager.Core.Swagger;
using DDS.SimpleTaskManager.Core.Telemetry;

using Microsoft.AspNetCore.Http.Json;

using Serilog;

using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace DDS.SimpleTaskManager.API.Api.Configuration;

public static class ApplicationConfiguration
{
    public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Host.AddSerilog(builder.Configuration);

        builder.Services.AddApplicationModule();
        builder.Services.AddInfrastructureModule(builder.Configuration);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddCors(options =>
        {
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            options.AddPolicy("Frontend",
                policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                    .WithHeaders("Content-Type", "Authorization")
                    .WithMethods("GET", "POST", "PATCH", "DELETE");
                });
        });

        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(options =>
        {
            options.UseCommonSwaggerDoc(builder.Configuration);
            options.AddEnumsWithValuesFixFilters();
        });

        return builder;
    }

    public static async Task ConfigureApplicationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("Frontend");
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseExceptionHandler();

        app.MapTaskItemEndpoints();

        if (app.Environment.IsDevelopment())
            await app.EnsureMigrationsAsync();

        await app.RunAsync();
    }
}