using DDS.SimpleTaskManager.API.Domain.TaskItems;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;

public sealed record GetTaskItemsQuery(
    TaskItemQueryFilter QueryFilter);