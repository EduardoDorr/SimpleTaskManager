using DDS.SimpleTaskManager.Core.Results.Errors;

namespace DDS.SimpleTaskManager.API.Domain.TaskItems;

public static class TaskItemError
{
    public static Error TitleIsRequired =>
        new("TaskItem.TitleIsRequired", "Title is required.", ErrorType.Validation);

    public static Error TitleIsTooLong =>
        new("TaskItem.TitleIsTooLong", "Title must be 50 characters or fewer.", ErrorType.Validation);

    public static Error DescriptionIsTooLong =>
        new("TaskItem.DescriptionIsTooLong", "Description must be 250 characters or fewer.", ErrorType.Validation);

    public static Error PriorityIsInvalid(string priority) =>
        new("TaskItem.PriorityIsInvalid", $"Priority '{priority}' is not valid.", ErrorType.Validation);

    public static Error DueDateIsInvalid =>
        new("TaskItem.DueDateIsInvalid", "Due Date must be today or later.", ErrorType.Validation);

    public static Error NotFound(long id) =>
        new("TaskItem.NotFound", $"Task '{id}' was not found.", ErrorType.NotFound);

    public static Error CreateFailed =>
        new("TaskItem.CreateFailed", "Could not create the TaskItem.", ErrorType.Failure);

    public static Error UpdateFailed(long id) =>
        new("TaskItem.UpdateFailed", $"Could not update task '{id}'.", ErrorType.Failure);

    public static Error DeleteFailed(long id) =>
        new("TaskItem.DeleteFailed", $"Could not delete task '{id}'.", ErrorType.Failure);
}