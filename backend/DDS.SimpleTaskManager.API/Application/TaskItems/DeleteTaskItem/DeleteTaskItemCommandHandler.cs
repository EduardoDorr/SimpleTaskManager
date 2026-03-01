using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.CommandQueries;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.DeleteTaskItem;

public interface IDeleteTaskItemCommandHandler
    : IRequestHandler<DeleteTaskItemCommand, Result>
{ }

public class DeleteTaskItemCommandHandler
    : BaseRequestHandler<DeleteTaskItemCommandHandler, DeleteTaskItemCommand, Result>,
    IDeleteTaskItemCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskItemCommandHandler(
        ILogger<DeleteTaskItemCommandHandler> logger,
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork) : base(logger)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result> ExecuteAsync(DeleteTaskItemCommand request, CancellationToken cancellationToken = default)
    {
        var taskItem =
            await _taskItemRepository
                .GetByIdAsync(
                    request.Id,
                    cancellationToken);

        if (taskItem is null)
        {
            _logger.LogWarning(
                "TaskItem with id {Id} could not be found.",
                request.Id);

            return Result.Fail(TaskItemError.NotFound(request.Id));
        }

        taskItem.Delete();

        _taskItemRepository.Update(taskItem);
        var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (rowsAffected <= 0)
        {
            _logger.LogError(
                "DeleteTask persistence failed. RowsAffected={RowsAffected}",
                rowsAffected);

            return Result.Fail(TaskItemError.DeleteFailed(request.Id));
        }

        _logger.LogInformation(
            "Task deleted. TaskId={TaskId} Priority={Priority} DueDate={DueDate}",
            taskItem.Id,
            taskItem.Priority,
            taskItem.DueDate);

        return Result.Ok();
    }
}