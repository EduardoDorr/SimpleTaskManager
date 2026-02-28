using Bogus;

using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;

using FluentAssertions;

namespace DDS.SimpleTaskManager.UnitTests.Domain.TaskItems;

public class TaskItemTests : BaseTest
{
    [Fact]
    public void Create_ShouldSucceed_AndNormalizeFields_WhenInputIsValid()
    {
        // Arrange
        var title = $"  {Faker.Random.String2(8, 20)}  ";
        var description = $"  {Faker.Lorem.Sentence(6)}  ";
        var dueDate = DateTime.UtcNow.AddDays(3);

        // Act
        var result = TaskItem
            .Create(
                title,
                description,
                Priority.High,
                dueDate);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be(title.Trim());
        result.Value.Description.Should().Be(description.Trim());
        result.Value.Status.Should().Be(TaskItemStatus.Backlog);
        result.Value.Priority.Should().Be(Priority.High);
    }

    [Fact]
    public void Create_ShouldFail_WhenTitleIsEmpty()
    {
        // Arrange
        // Act
        var result = TaskItem
            .Create(
                "   ",
                Faker.Lorem.Sentence(),
                Priority.Medium,
                DateTime.UtcNow.AddDays(1));

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.TitleIsRequired");
    }

    [Fact]
    public void Create_ShouldFail_WhenPriorityIsInvalid()
    {
        // Arrange
        var invalidPriority = (Priority)999;

        // Act
        var result = TaskItem
            .Create(
                Faker.Random.String2(10),
                Faker.Lorem.Sentence(),
                invalidPriority,
                DateTime.UtcNow.AddDays(1));

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.PriorityIsInvalid");
    }

    [Fact]
    public void Create_ShouldFail_WhenDueDateIsInThePast()
    {
        // Arrange
        // Act
        var result = TaskItem
            .Create(
                Faker.Random.String2(10),
                Faker.Lorem.Sentence(),
                Priority.Low,
                DateTime.UtcNow.AddDays(-1));

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.DueDateIsInvalid");
    }

    [Fact]
    public void ToggleCompletion_ShouldAlternateBetweenBacklogAndCompleted()
    {
        // Arrange
        // Act
        var taskItem = TaskItem
            .Create(
                Faker.Random.String2(10),
                Faker.Lorem.Sentence(),
                Priority.Critical,
                DateTime.UtcNow.AddDays(7)).Value!;

        // Assert
        taskItem.Status.Should().Be(TaskItemStatus.Backlog);

        taskItem.ToggleCompletion();
        taskItem.Status.Should().Be(TaskItemStatus.Completed);

        taskItem.ToggleCompletion();
        taskItem.Status.Should().Be(TaskItemStatus.Backlog);
    }
}