using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Results.Base;

namespace DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;

public sealed record CreateTaskItemCommand(
    string Title,
    string? Description,
    Priority Priority,
    DateTime DueDate);

public static class CreateTaskItemCommandExtensions
{
    public static Result<TaskItem> ToTaskItemResult(this CreateTaskItemCommand command)
        => TaskItem.Create(
            command.Title,
            command.Description,
            command.Priority,
            command.DueDate);
}