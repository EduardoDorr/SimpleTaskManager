using DDS.SimpleTaskManager.API.Application.TaskItems.Models;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.CommandQueries;
using DDS.SimpleTaskManager.Core.Models.Pagination;
using DDS.SimpleTaskManager.Core.Results.Base;
using DDS.SimpleTaskManager.Core.Results.FluentValidation;

using FluentValidation;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;

public interface IGetTaskItemsQueryHandler
    : IRequestHandler<GetTaskItemsQuery, Result<PaginationResult<TaskItemViewModel>>>
{ }

public class GetTaskItemsQueryHandler
    : BaseRequestHandler<GetTaskItemsQueryHandler, GetTaskItemsQuery, Result<PaginationResult<TaskItemViewModel>>>,
    IGetTaskItemsQueryHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IValidator<GetTaskItemsQuery> _validator;

    public GetTaskItemsQueryHandler(
        ILogger<GetTaskItemsQueryHandler> logger,
        ITaskItemRepository taskItemRepository,
        IValidator<GetTaskItemsQuery> validator) : base(logger)
    {
        _taskItemRepository = taskItemRepository;
        _validator = validator;
    }

    protected override async Task<Result<PaginationResult<TaskItemViewModel>>> ExecuteAsync(GetTaskItemsQuery request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.ToErrors();
            _logger.LogWarning(
                "QueryFilter has errors. ErrorCodes={ErrorCodes}",
                errors.Select(e => e.Code));

            return Result.Fail<PaginationResult<TaskItemViewModel>>(errors);
        }

        var taskItems =
            await _taskItemRepository
                .GetAllAsync(
                    request.QueryFilter,
                    cancellationToken);

        var paginatedTaskItems =
            taskItems
                .Map(taskItems.Data.ToModel().ToList());

        _logger.LogInformation(
            "Tasks retrieved. Page={Page} PageSize={PageSize} IsDescending={IsDescending} IsActive={IsActive}",
            request.QueryFilter.Page,
            request.QueryFilter.PageSize,
            request.QueryFilter.IsDescending,
            request.QueryFilter.IsActive);

        return Result.Ok(paginatedTaskItems);
    }
}