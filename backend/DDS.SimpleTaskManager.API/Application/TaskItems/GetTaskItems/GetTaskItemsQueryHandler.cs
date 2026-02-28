using DDS.SimpleTaskManager.API.Application.TaskItems.Models;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Interfaces;
using DDS.SimpleTaskManager.Core.Models.Pagination;
using DDS.SimpleTaskManager.Core.Results.Base;
using DDS.SimpleTaskManager.Core.Results.FluentValidation;

using FluentValidation;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;

public interface IGetTaskItemsQueryHandler
    : IRequestHandler<GetTaskItemsQuery, Result<PaginationResult<TaskItemViewModel>>>
{ }

public class GetTaskItemsQueryHandler : IGetTaskItemsQueryHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IValidator<GetTaskItemsQuery> _validator;

    public GetTaskItemsQueryHandler(
        ITaskItemRepository taskItemRepository,
        IValidator<GetTaskItemsQuery> validator)
    {
        _taskItemRepository = taskItemRepository;
        _validator = validator;
    }

    public async Task<Result<PaginationResult<TaskItemViewModel>>> HandleAsync(GetTaskItemsQuery request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result.Fail<PaginationResult<TaskItemViewModel>>(validation.ToErrors());

        var taskItems =
            await _taskItemRepository
                .GetAllAsync(
                    request.QueryFilter,
                    cancellationToken);

        var paginatedTaskItems =
            taskItems
                .Map(taskItems.Data.ToModel().ToList());

        return Result.Ok(paginatedTaskItems);
    }
}