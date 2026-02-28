using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Interfaces;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;

public interface ICreateTaskItemCommandHandler
    : IRequestHandler<CreateTaskItemCommand, Result<long>>
{ }

public class CreateTaskItemCommandHandler : ICreateTaskItemCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<long>> HandleAsync(CreateTaskItemCommand request, CancellationToken cancellationToken = default)
    {
        var taskItemResult = request.ToTaskItemResult();
        if (!taskItemResult.Success)
            return Result.Fail<long>(taskItemResult.Errors);

        var taskItem = taskItemResult.Value!;

        _taskItemRepository.Create(taskItem);
        var createResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (createResult <= 0)
            return Result.Fail<long>(TaskItemError.CreateFailed);

        return Result.Ok(taskItem.Id);
    }
}