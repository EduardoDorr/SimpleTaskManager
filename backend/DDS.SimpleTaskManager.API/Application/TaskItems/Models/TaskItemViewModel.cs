using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.Models;

public sealed record TaskItemViewModel(
    long Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    Priority Priority,
    DateTime DueDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive);

public static class TaskItemViewModelExtensions
{
    public static TaskItemViewModel? ToModel(this TaskItem taskItem)
        => taskItem is null
            ? null
            : new(
                taskItem.Id,
                taskItem.Title,
                taskItem.Description,
                taskItem.Status,
                taskItem.Priority,
                taskItem.DueDate,
                taskItem.CreatedAt,
                taskItem.UpdatedAt,
                taskItem.IsActive);

    public static IEnumerable<TaskItemViewModel> ToModel(this IEnumerable<TaskItem> taskItems)
        => taskItems is null
            ? []
            : taskItems.Where(ti => ti is not null).Select(ti => ti.ToModel()!);
}