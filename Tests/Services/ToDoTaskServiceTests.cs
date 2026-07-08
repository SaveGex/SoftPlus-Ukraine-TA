using Application.DTOs;
using Application.Services;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories.Interfaces;
using FluentAssertions;
using MapsterMapper;
using Moq;
namespace Tests.Services;

public class ToDoTaskServiceTests
{
    private readonly Mock<IToDoTaskRepository> _repositoryMock;
    private readonly ToDoTaskService _sut;

    public ToDoTaskServiceTests()
    {
        _repositoryMock = new Mock<IToDoTaskRepository>();
        _sut = new ToDoTaskService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateToDoTaskAsync_WithValidDto_ShouldReturnResponseDto()
    {
        // Arrange
        var createDto = new ToDoTaskCreateDTO("Clean my room", "Vacuum the floor", false, false, null, null, RecurrenceType.None, null);
        var domainTask = new ToDoTask { Id = Guid.NewGuid(), Title = createDto.Title, Note = createDto.Note };

        _repositoryMock.Setup(r => r.CreateToDoTaskAsync(It.IsAny<ToDoTask>())).ReturnsAsync(domainTask);

        // Act
        var result = await _sut.CreateToDoTaskAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(domainTask.Id);
        result.Title.Should().Be(domainTask.Title);
        _repositoryMock.Verify(r => r.CreateToDoTaskAsync(It.IsAny<ToDoTask>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CreateToDoTaskAsync_WithInvalidTitle_ShouldThrowException(string invalidTitle)
    {
        // Arrange
        var invalidDto = new ToDoTaskCreateDTO(invalidTitle, "Note", false, false, null, null, RecurrenceType.None, null);

        // Act
        var act = () => _sut.CreateToDoTaskAsync(invalidDto);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Task title cannot be empty.");
        _repositoryMock.Verify(r => r.CreateToDoTaskAsync(It.IsAny<ToDoTask>()), Times.Never);
    }

    [Fact]
    public async Task UpdateToDoTaskAsync_WhenTaskDoesNotExist_ShouldThrowException()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var updateDto = new ToDoTaskUpdateDTO("Updated Title", null, false, false, false, null, null, RecurrenceType.None, null);
        _repositoryMock.Setup(r => r.GetToDoTaskByIdAsync(id));

        // Act
        var act = () => _sut.UpdateToDoTaskAsync(id, updateDto);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"ToDoTask with id {id} was not found to update.");
        _repositoryMock.Verify(r => r.UpdateToDoTaskAsync(It.IsAny<ToDoTask>()), Times.Never);
    }

    [Fact]
    public async Task DeleteToDoTaskAsync_WhenTaskExists_ShouldReturnResponseDto()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = taskId, Title = "To Delete" };

        _repositoryMock.Setup(r => r.GetToDoTaskByIdAsync(taskId)).ReturnsAsync(existingTask);
        _repositoryMock.Setup(r => r.DeleteToDoTaskAsync(It.IsAny<ToDoTask>())).ReturnsAsync(existingTask);

        // Act
        var result = await _sut.DeleteToDoTaskAsync(taskId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(taskId);
        _repositoryMock.Verify(r => r.DeleteToDoTaskAsync(It.IsAny<ToDoTask>()), Times.Once);
    }

    [Fact]
    public async Task DeleteToDoTaskAsync_WhenTaskDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetToDoTaskByIdAsync(nonExistingId)).ReturnsAsync((ToDoTask)null!);

        // Act
        var act = () => _sut.DeleteToDoTaskAsync(nonExistingId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"ToDoTask with id {nonExistingId} was not found.");
        _repositoryMock.Verify(r => r.DeleteToDoTaskAsync(It.IsAny<ToDoTask>()), Times.Never);
    }
}