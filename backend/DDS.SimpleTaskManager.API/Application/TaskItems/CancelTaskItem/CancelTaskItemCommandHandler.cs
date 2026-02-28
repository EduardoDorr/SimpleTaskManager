using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Interfaces;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.CancelTaskItem;

public interface ICancelTaskItemCommandHandler
    : IRequestHandler<CancelTaskItemCommand, Result>
{ }

public class CancelTaskItemCommandHandler : ICancelTaskItemCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CancelTaskItemCommand request, CancellationToken cancellationToken = default)
    {
        var taskItem =
            await _taskItemRepository
                .GetByIdAsync(
                    request.Id,
                    cancellationToken);

        if (taskItem is null)
            return Result.Fail(TaskItemError.NotFound(request.Id));

        taskItem.Delete();

        _taskItemRepository.Update(taskItem);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveResult <= 0)
            return Result.Fail(TaskItemError.DeleteFailed(request.Id));

        return Result.Ok();
    }
}