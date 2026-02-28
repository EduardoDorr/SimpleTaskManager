using DDS.SimpleTaskManager.Core.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace DDS.SimpleTaskManager.Core.Swagger;

public static class SwaggerExtensions
{
    public static void UseCommonSwaggerDoc(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var apiConfiguration = configuration
            .GetSection(ApiConfigurationOptions.Name)
            .Get<ApiConfigurationOptions>();

        ArgumentNullException.ThrowIfNull(apiConfiguration, nameof(apiConfiguration));

        foreach (var apiVersion in apiConfiguration.ApiVersions)
        {
            options.UseCommonSwaggerDoc(
                apiVersion.Name,
                apiVersion.Version,
                apiConfiguration.ApiContact.Name,
                apiConfiguration.ApiContact.Email,
                apiConfiguration.ApiContact.Url);
        }
        options.AddEnumsWithValuesFixFilters();
    }

    public static void UseCommonSwaggerDoc(this SwaggerGenOptions options, string apiName, string apiVersion, string authorName, string authorEmail, string authorUrl)
    {
        options.SwaggerDoc(apiVersion, new OpenApiInfo
        {
            Title = apiName,
            Version = apiVersion,
            Contact = new OpenApiContact
            {
                Name = authorName,
                Email = authorEmail,
                Url = new Uri(authorUrl)
            }
        });
    }
}