using System.Reflection;

using DDS.SimpleTaskManager.API.Application.TaskItems.ChangeStatusTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.DeleteTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;

using FluentValidation;

namespace DDS.SimpleTaskManager.API.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services
            .AddServices()
            .AddFluentValidation();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGetTaskItemsQueryHandler, GetTaskItemsQueryHandler>();
        services.AddScoped<ICreateTaskItemCommandHandler, CreateTaskItemCommandHandler>();
        services.AddScoped<IChangeStatusTaskItemCommandHandler, ChangeStatusTaskItemCommandHandler>();
        services.AddScoped<IDeleteTaskItemCommandHandler, DeleteTaskItemCommandHandler>();

        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}