using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.CommandQueries;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;

public interface ICreateTaskItemCommandHandler
    : IRequestHandler<CreateTaskItemCommand, Result<long>>
{ }

public class CreateTaskItemCommandHandler
    : BaseRequestHandler<CreateTaskItemCommandHandler, CreateTaskItemCommand, Result<long>>,
    ICreateTaskItemCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskItemCommandHandler(
        ILogger<CreateTaskItemCommandHandler> logger,
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork) : base(logger)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<long>> ExecuteAsync(CreateTaskItemCommand request, CancellationToken cancellationToken = default)
    {
        var taskItemResult = request.ToTaskItemResult();
        if (!taskItemResult.Success)
        {
            _logger.LogWarning(
                "TaskItem cannot be created. ErrorCodes={ErrorCodes}",
                taskItemResult.Errors.Select(e => e.Code));

            return Result.Fail<long>(taskItemResult.Errors);
        }

        var taskItem = taskItemResult.Value!;

        _taskItemRepository.Create(taskItem);
        var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (rowsAffected <= 0)
        {
            _logger.LogError(
                "CreateTask persistence failed. RowsAffected={RowsAffected}",
                rowsAffected);

            return Result.Fail<long>(TaskItemError.CreateFailed);
        }

        _logger.LogInformation(
            "Task created. TaskId={TaskId} Priority={Priority} DueDate={DueDate}",
            taskItem.Id,
            taskItem.Priority,
            taskItem.DueDate);

        return Result.Ok(taskItem.Id);
    }
}