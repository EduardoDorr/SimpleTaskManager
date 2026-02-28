using System.Text.Json.Serialization;

using DDS.SimpleTaskManager.API.Api.Endpoints;
using DDS.SimpleTaskManager.API.Application;
using DDS.SimpleTaskManager.API.Infrastructure;
using DDS.SimpleTaskManager.Core.Middlewares;
using DDS.SimpleTaskManager.Core.Swagger;
using DDS.SimpleTaskManager.Core.Telemetry;

using Microsoft.AspNetCore.Http.Json;

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
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
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

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseExceptionHandler();

        app.MapTaskItemEndpoints();

        await app.EnsureMigrationsAsync();

        await app.RunAsync();
    }
}