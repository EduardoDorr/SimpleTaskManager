using DDS.SimpleTaskManager.API.Api.Configuration;

await WebApplication.CreateBuilder(args)
    .ConfigureApplicationServices().Build()
    .ConfigureApplicationPipeline();