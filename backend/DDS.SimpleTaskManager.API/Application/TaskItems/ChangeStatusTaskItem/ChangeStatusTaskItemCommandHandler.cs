using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Interfaces;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.ChangeStatusTaskItem;

public interface IChangeStatusTaskItemCommandHandler
    : IRequestHandler<ChangeStatusTaskItemCommand, Result>
{ }

public class ChangeStatusTaskItemCommandHandler : IChangeStatusTaskItemCommandHandler
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeStatusTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork)
    {
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(ChangeStatusTaskItemCommand request, CancellationToken cancellationToken = default)
    {
        var taskItem =
            await _taskItemRepository
                .GetByIdAsync(
                    request.Id,
                    cancellationToken);

        if (taskItem is null)
            return Result.Fail(TaskItemError.NotFound(request.Id));

        taskItem.ToggleCompletion();

        _taskItemRepository.Update(taskItem);
        var updateResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (updateResult <= 0)
            return Result.Fail(TaskItemError.UpdateFailed(request.Id));

        return Result.Ok();
    }
}