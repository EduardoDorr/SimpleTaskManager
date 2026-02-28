using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.API.Infrastructure.Persistence;
using DDS.SimpleTaskManager.API.Infrastructure.Persistence.Repositories;
using DDS.SimpleTaskManager.Core.Persistence.Interceptors;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace DDS.SimpleTaskManager.API.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories();

        return services;
    }

    public static async Task<WebApplication> EnsureMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(TaskManagerDbContext.Name);

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<TaskManagerDbContext>(options =>
            options.UseMySQL(
                connectionString)
            .AddInterceptors(new HardDeletePreventionInterceptor()));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TaskManagerDbContext>>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        return services;
    }
}