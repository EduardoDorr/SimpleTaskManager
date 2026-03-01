using Bogus;

using DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDS.SimpleTaskManager.UnitTests.Application.TaskItems.CreateTaskItem;

public class CreateTaskItemCommandHandlerTests : BaseTest
{
    private readonly Mock<ILogger<CreateTaskItemCommandHandler>> _loggerMock = new();
    private readonly Mock<ITaskItemRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTests()
    {
        _handler = new CreateTaskItemCommandHandler(
            _loggerMock.Object,
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateTask_WhenCommandIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = BuildValidCommand();

        _repositoryMock.Setup(r => r.Create(It.IsAny<TaskItem>()))
            .Verifiable(Times.Once);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable(Times.Once);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        
        _repositoryMock.VerifyAll();
        _unitOfWorkMock.VerifyAll();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenDomainValidationFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = BuildValidCommand() with { Title = "  " };

        _repositoryMock.Setup(r => r.Create(It.IsAny<TaskItem>()))
            .Verifiable(Times.Never);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Verifiable(Times.Never);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.TitleIsRequired");
        
        _repositoryMock.VerifyAll();
        _unitOfWorkMock.VerifyAll();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenPersistenceReturnsZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = BuildValidCommand();
        
        _repositoryMock.Setup(r => r.Create(It.IsAny<TaskItem>()))
            .Verifiable(Times.Once);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0)
            .Verifiable(Times.Once);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "TaskItem.CreateFailed");
    }

    private CreateTaskItemCommand BuildValidCommand()
        => new(
            Faker.Random.String2(8, 30),
            Faker.Lorem.Sentence(8),
            Faker.PickRandom<Priority>(),
            DateTime.UtcNow.AddDays(Faker.Random.Int(1, 30)));
}