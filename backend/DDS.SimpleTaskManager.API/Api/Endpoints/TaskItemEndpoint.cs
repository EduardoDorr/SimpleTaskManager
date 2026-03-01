using System.Net;

using DDS.SimpleTaskManager.API.Application.TaskItems.CancelTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.ChangeStatusTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;
using DDS.SimpleTaskManager.API.Application.TaskItems.Models;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Models.Pagination;
using DDS.SimpleTaskManager.Core.Results.Api;
using DDS.SimpleTaskManager.Core.Results.Extensions;

namespace DDS.SimpleTaskManager.API.Api.Endpoints;

public static class TaskItemEndpoint
{
    private const string ROUTE = "/api/v1/tasks";

    public static WebApplication MapTaskItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup(ROUTE)
            .WithTags("Tasks");

        group.MapGet("/", async (
            [AsParameters] TaskItemQueryFilter queryFilter,
            IGetTaskItemsQueryHandler service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.HandleAsync(new(queryFilter), cancellationToken);
            return result.ToApiResult().ToResult();
        })
        .Produces<ApiResult<PaginationResult<TaskItemViewModel>>>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError);

        group.MapPost("/", async (
            CreateTaskItemCommand command,
            ICreateTaskItemCommandHandler service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.HandleAsync(command, cancellationToken);
            return result.ToApiResult(HttpStatusCode.Created).ToResult();
        })
        .Produces<ApiResult<long>>(StatusCodes.Status201Created)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError);

        group.MapPatch("/{id}/status", async (
            long id,
            IChangeStatusTaskItemCommandHandler service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.HandleAsync(new(id), cancellationToken);
            return result.ToApiResult(HttpStatusCode.OK).ToResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status404NotFound)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError);

        group.MapDelete("/{id}", async (
            long id,
            ICancelTaskItemCommandHandler service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.HandleAsync(new(id), cancellationToken);
            return result.ToApiResult(HttpStatusCode.OK).ToResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status404NotFound)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError);

        return app;
    }
}