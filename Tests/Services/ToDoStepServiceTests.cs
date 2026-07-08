using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services;
using Domain.Models;
using Domain.Repositories.Interfaces;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class ToDoStepServiceTests
    {
        [Fact]
        public async Task Constructor_WithValidDependencies_ServiceUsesProvidedInstances()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();

            var createDto = new ToDoStepCreateDTO("title", Guid.NewGuid());
            var createdStep = new ToDoStep { Id = Guid.NewGuid(), Title = "title", TodoTaskId = createDto.TodoTaskId };

            repoMock.Setup(r => r.CreateToDoStepAsync(It.IsAny<ToDoStep>())).ReturnsAsync(createdStep);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.CreateToDoStepAsync(createDto);

            // Assert - behavior shows constructor correctly stored dependencies
            result.Should().NotBeNull();
            result.Id.Should().Be(createdStep.Id);
            result.Title.Should().Be(createdStep.Title);
            repoMock.Verify(r => r.CreateToDoStepAsync(It.IsAny<ToDoStep>()), Times.Once);
        }

        [Fact]
        public async Task CreateToDoStepAsync_WithEmptyTitle_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var service = new ToDoStepService(repoMock.Object);

            var dto = new ToDoStepCreateDTO("    ", Guid.NewGuid());

            // Act
            Func<Task> act = async () => await service.CreateToDoStepAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Step name cannot be empty.");
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateToDoStepAsync_WithValidDto_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();

            var dto = new ToDoStepCreateDTO("Do it", Guid.NewGuid());
            var created = new ToDoStep { Id = Guid.NewGuid(), Title = dto.Title, TodoTaskId = dto.TodoTaskId };

            repoMock.Setup(r => r.CreateToDoStepAsync(It.IsAny<ToDoStep>())).ReturnsAsync(created);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.CreateToDoStepAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(created.Id);
            result.Title.Should().Be(created.Title);
            repoMock.Verify(r => r.CreateToDoStepAsync(It.IsAny<ToDoStep>()), Times.Once);
        }

        [Fact]
        public async Task GetToDoStepByIdAsync_WhenNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var id = Guid.NewGuid();
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync((ToDoStep)null!);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            Func<Task> act = async () => await service.GetToDoStepByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep with id {id} was not found.");
        }

        [Fact]
        public async Task GetToDoStepByIdAsync_WhenFound_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var id = Guid.NewGuid();
            var step = new ToDoStep { Id = id, Title = "t", TodoTaskId = Guid.NewGuid() };

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(step);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.GetToDoStepByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be("t");
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetToDoStepsByTaskIdAsync_ReturnsMappedCollection()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var taskId = Guid.NewGuid();

            var steps = new List<ToDoStep>
            {
                new ToDoStep { Id = Guid.NewGuid(), Title = "a", TodoTaskId = taskId },
                new ToDoStep { Id = Guid.NewGuid(), Title = "b", TodoTaskId = taskId }
            };

            repoMock.Setup(r => r.GetToDoStepsByTaskIdAsync(taskId)).ReturnsAsync(steps);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.GetToDoStepsByTaskIdAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            repoMock.Verify(r => r.GetToDoStepsByTaskIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WithEmptyTitle_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var service = new ToDoStepService(repoMock.Object);

            Guid id = Guid.NewGuid();
            var dto = new ToDoStepUpdateDTO(" ", false);

            // Act
            Func<Task> act = async () => await service.UpdateToDoStepAsync(id, dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Step name cannot be empty.");
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WhenExistingNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var id = Guid.NewGuid();
            var dto = new ToDoStepUpdateDTO("title", false);
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id));

            var service = new ToDoStepService(repoMock.Object);

            // Act
            Func<Task> act = async () => await service.UpdateToDoStepAsync(id, dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep with id {id} was not found to update.");
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WithValidDto_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();

            var id = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var dto = new ToDoStepUpdateDTO("new title", true);

            var existing = new ToDoStep { Id = id, Title = "old", TodoTaskId = taskId};
            var updated = new ToDoStep { Id = id, Title = dto.Title, IsCompleted = dto.IsCompleted, TodoTaskId = taskId };

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(existing);
            repoMock.Setup(r => r.UpdateToDoStepAsync(It.IsAny<ToDoStep>())).ReturnsAsync(updated);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.UpdateToDoStepAsync(id, dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(updated.Id);
            result.Title.Should().Be(updated.Title);
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            repoMock.Verify(r => r.UpdateToDoStepAsync(It.IsAny<ToDoStep>()), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoStepAsync_WhenNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var id = Guid.NewGuid();
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync((ToDoStep)null!);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            Func<Task> act = async () => await service.DeleteToDoStepAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep with id {id} was not found.");
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteToDoStepAsync_WhenFound_DeletesAndReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();

            var id = Guid.NewGuid();
            var step = new ToDoStep { Id = id, Title = "to delete", TodoTaskId = Guid.NewGuid() };
            var deleted = new ToDoStep { Id = id, Title = "to delete", TodoTaskId = step.TodoTaskId };

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(step);
            repoMock.Setup(r => r.DeleteToDoStepAsync(step)).ReturnsAsync(deleted);

            var service = new ToDoStepService(repoMock.Object);

            // Act
            var result = await service.DeleteToDoStepAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be("to delete");
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            repoMock.Verify(r => r.DeleteToDoStepAsync(step), Times.Once);
        }
    }
}
