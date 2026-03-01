using Bogus;

using DDS.SimpleTaskManager.API.Application.TaskItems.ChangeStatusTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.CreateTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.DeleteTaskItem;
using DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.API.Infrastructure.Persistence;
using DDS.SimpleTaskManager.API.Infrastructure.Persistence.Repositories;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Persistence.UnitOfWork;
using DDS.SimpleTaskManager.IntegrationTests.Infrastructure;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDS.SimpleTaskManager.IntegrationTests.TaskItems;

public sealed class TaskItemUseCasesIntegrationTests : IClassFixture<MySqlTaskManagerFixture>
{
    private static readonly Faker Faker = new();

    private readonly MySqlTaskManagerFixture _fixture;

    public TaskItemUseCasesIntegrationTests(MySqlTaskManagerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAndGetAndChangeStatusAndDelete_ShouldPersistAndReturn()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var dbContext = _fixture.CreateDbContext();
        using var unitOfWork = new UnitOfWork<TaskManagerDbContext>(dbContext);
        var repository = new TaskItemRepository(dbContext);

        var createCommand = BuildValidCreateCommand();

        // Act
        // Assert
        var createdId = await CreateOperation(unitOfWork, repository, createCommand);

        await GetAllOperation(createdId, repository, createCommand);

        await ChangeStatusOperation(createdId, unitOfWork, repository);

        await DeleteOperation(createdId, unitOfWork, repository);
    }

    private static async Task<long> CreateOperation(UnitOfWork<TaskManagerDbContext> unitOfWork, TaskItemRepository repository, CreateTaskItemCommand createCommand)
    {
        // Arrange
        var logger = new Mock<ILogger<CreateTaskItemCommandHandler>>();
        var createHandler = new CreateTaskItemCommandHandler(logger.Object, repository, unitOfWork);

        // Act
        var createResult = await createHandler.HandleAsync(createCommand);

        // Assert
        createResult.Success.Should().BeTrue();
        createResult.Value.Should().BeGreaterThan(0);

        return createResult.Value;
    }

    private static async Task GetAllOperation(long createdId, TaskItemRepository repository, CreateTaskItemCommand createCommand)
    {
        // Arrange
        var logger = new Mock<ILogger<GetTaskItemsQueryHandler>>();
        var getHandler = new GetTaskItemsQueryHandler(logger.Object, repository, new GetTaskItemsQueryValidator());
        var query = new GetTaskItemsQuery(new TaskItemQueryFilter(Page: 1, PageSize: 10));

        // Act
        var getResult = await getHandler.HandleAsync(query);

        // Assert
        getResult.Success.Should().BeTrue();
        getResult.Value.Should().NotBeNull();
        getResult.Value!.Data.Should().ContainSingle(item =>
            item.Id == createdId &&
            item.Title == createCommand.Title.Trim());
    }

    private static async Task DeleteOperation(long createdId, UnitOfWork<TaskManagerDbContext> unitOfWork, TaskItemRepository repository)
    {
        // Arrange
        var logger = new Mock<ILogger<DeleteTaskItemCommandHandler>>();
        var cancelHandler = new DeleteTaskItemCommandHandler(logger.Object, repository, unitOfWork);

        // Act
        var cancelResult = await cancelHandler.HandleAsync(new DeleteTaskItemCommand(createdId));
        var canceledTask = await repository.GetByIdAsync(createdId);

        // Assert
        cancelResult.Success.Should().BeTrue();
        canceledTask.Should().BeNull();
    }

    private static async Task ChangeStatusOperation(long createdId, UnitOfWork<TaskManagerDbContext> unitOfWork, TaskItemRepository repository)
    {
        // Arrange
        var logger = new Mock<ILogger<ChangeStatusTaskItemCommandHandler>>();
        var changeStatusHandler = new ChangeStatusTaskItemCommandHandler(logger.Object, repository, unitOfWork);

        // Act
        var statusResult = await changeStatusHandler.HandleAsync(new ChangeStatusTaskItemCommand(createdId));

        // Assert
        statusResult.Success.Should().BeTrue();
    }

    private static CreateTaskItemCommand BuildValidCreateCommand()
        => new(
            Faker.Random.String2(8, 30),
            Faker.Lorem.Sentence(8),
            Faker.PickRandom<Priority>(),
            DateTime.UtcNow.AddDays(Faker.Random.Int(1, 30)));
}