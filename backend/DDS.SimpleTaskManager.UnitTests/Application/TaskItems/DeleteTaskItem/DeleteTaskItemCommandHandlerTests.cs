using Bogus;

using DDS.SimpleTaskManager.API.Application.TaskItems.DeleteTaskItem;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDS.SimpleTaskManager.UnitTests.Application.TaskItems.DeleteTaskItem;

public class DeleteTaskItemCommandHandlerTests : BaseTest
{
    private readonly Mock<ILogger<DeleteTaskItemCommandHandler>> _loggerMock = new();
    private readonly Mock<ITaskItemRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteTaskItemCommandHandler _handler;

    public DeleteTaskItemCommandHandlerTests()
    {
        _handler = new DeleteTaskItemCommandHandler(
            _loggerMock.Object,
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenTaskDoesNotExist()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _repositoryMock.Setup(r => r.GetByIdAsync(31, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null)
            .Verifiable(Times.Once);

        _repositoryMock.Setup(r => r.Update(It.IsAny<TaskItem>()))
            .Verifiable(Times.Never);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Verifiable(Times.Never);

        // Act
        var result = await _handler.HandleAsync(new DeleteTaskItemCommand(31), cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.NotFound");

        _repositoryMock.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldSoftDeleteTask_WhenTaskExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var taskItem = BuildTaskItem();

        _repositoryMock.Setup(r => r.GetByIdAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem)
            .Verifiable(Times.Once);

        _repositoryMock.Setup(r => r.Update(taskItem))
            .Verifiable(Times.Once);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable(Times.Once);

        // Act
        var result = await _handler.HandleAsync(new DeleteTaskItemCommand(20), cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        taskItem.IsDeleted.Should().BeTrue();

        _repositoryMock.VerifyAll();
        _unitOfWorkMock.VerifyAll();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPersistenceReturnsZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var taskItem = BuildTaskItem();

        _repositoryMock.Setup(r => r.GetByIdAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem)
            .Verifiable(Times.Once);

        _repositoryMock.Setup(r => r.Update(taskItem))
            .Verifiable(Times.Once);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable(Times.Once);

        // Act
        var result = await _handler.HandleAsync(new DeleteTaskItemCommand(20), cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.DeleteFailed");
    }

    private TaskItem BuildTaskItem()
        => TaskItem.Create(
            Faker.Random.String2(8, 20),
            Faker.Lorem.Sentence(),
            Priority.High,
            DateTime.UtcNow.AddDays(Faker.Random.Int(0, 100))).Value!;
}