using Bogus;

using DDS.SimpleTaskManager.API.Application.TaskItems.GetTaskItems;
using DDS.SimpleTaskManager.API.Domain.TaskItems;
using DDS.SimpleTaskManager.Core.Enums;
using DDS.SimpleTaskManager.Core.Models.Pagination;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using Moq;

using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DDS.SimpleTaskManager.UnitTests.Application.TaskItems.GetTaskItems;

public class GetTaskItemsQueryHandlerTests : BaseTest
{
    private readonly Mock<ITaskItemRepository> _repositoryMock = new();
    private readonly Mock<IValidator<GetTaskItemsQuery>> _validatorMock = new();

    private readonly GetTaskItemsQueryHandler _handler;

    public GetTaskItemsQueryHandlerTests()
    {
        _handler = new GetTaskItemsQueryHandler(
            _repositoryMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnMappedPage_WhenRequestIsValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var filter = new TaskItemQueryFilter(Page: 1, PageSize: 10);
        var query = new GetTaskItemsQuery(filter);

        var taskItemA = TaskItem
            .Create(
                Faker.Random.String2(8, 20),
                Faker.Lorem.Sentence(),
                Priority.High,
                DateTime.UtcNow.AddDays(2)).Value!;

        var taskItemB = TaskItem
            .Create(
                Faker.Random.String2(8, 20),
                Faker.Lorem.Sentence(),
                Priority.Low,
                DateTime.UtcNow.AddDays(3)).Value!;

        var repositoryResult = new PaginationResult<TaskItem>(1, 10, 2, 1, [taskItemA, taskItemB]);

        _validatorMock
            .Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult())
            .Verifiable(Times.Once);

        _repositoryMock
            .Setup(r => r.GetAllAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryResult)
            .Verifiable(Times.Once);

        // Act
        var result = await _handler.HandleAsync(query, cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.TotalCount.Should().Be(2);
        result.Value.Data.Should().HaveCount(2);
        result.Value.Data.Select(d => d.Title).Should().Contain([taskItemA.Title, taskItemB.Title]);

        _repositoryMock.VerifyAll();
        _validatorMock.VerifyAll();
    }

    [Fact]
    public async Task HandleAsync_ShouldFail_WhenValidationFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var filter = new TaskItemQueryFilter(Page: 0, PageSize: 10);
        var query = new GetTaskItemsQuery(filter);

        var validationFailure = new ValidationFailure("QueryFilter.Page", PaginationError.PageMustBeGreaterThanZero.Message)
        {
            ErrorCode = PaginationError.PageMustBeGreaterThanZero.Code,
            CustomState = PaginationError.PageMustBeGreaterThanZero
        };

        _validatorMock
           .Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
           .ReturnsAsync(new ValidationResult([validationFailure]))
           .Verifiable(Times.Once);

        _repositoryMock
            .Setup(r => r.GetAllAsync(filter, It.IsAny<CancellationToken>()))
            .Verifiable(Times.Never);

        // Act
        var result = await _handler.HandleAsync(query, cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "Pagination.PageMustBeGreaterThanZero");

        _repositoryMock.VerifyAll();
        _validatorMock.VerifyAll();
    }
}