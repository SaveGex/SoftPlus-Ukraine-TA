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
            var mapperMock = new Mock<IMapper>();

            var createDto = new ToDoStepCreateDTO("title", Guid.NewGuid());
            var mappedStep = new ToDoStep { Id = Guid.NewGuid(), Title = "title", TodoTaskId = createDto.TodoTaskId };
            var createdStep = new ToDoStep { Id = Guid.NewGuid(), Title = "title", TodoTaskId = createDto.TodoTaskId };
            var response = new ToDoStepResponseDTO(createdStep.Id, createdStep.Title, createdStep.IsCompleted, createdStep.TodoTaskId);

            mapperMock.Setup(m => m.Map<ToDoStep>(It.IsAny<ToDoStepCreateDTO>())).Returns(mappedStep);
            repoMock.Setup(r => r.CreateToDoStepAsync(mappedStep)).ReturnsAsync(createdStep);
            mapperMock.Setup(m => m.Map<ToDoStepResponseDTO>(createdStep)).Returns(response);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.CreateToDoStepAsync(createDto);

            // Assert - behavior shows constructor correctly stored dependencies
            result.Should().BeSameAs(response);
            mapperMock.Verify(m => m.Map<ToDoStep>(createDto), Times.Once);
            repoMock.Verify(r => r.CreateToDoStepAsync(mappedStep), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStepResponseDTO>(createdStep), Times.Once);
        }

        [Fact]
        public async Task CreateToDoStepAsync_WithEmptyTitle_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            var dto = new ToDoStepCreateDTO("   ", Guid.NewGuid());

            // Act
            Func<Task> act = async () => await service.CreateToDoStepAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Step name cannot be empty.");
            repoMock.VerifyNoOtherCalls();
            mapperMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateToDoStepAsync_WithValidDto_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();

            var dto = new ToDoStepCreateDTO("Do it", Guid.NewGuid());
            var step = new ToDoStep { Id = Guid.NewGuid(), Title = dto.Title, TodoTaskId = dto.TodoTaskId };
            var created = new ToDoStep { Id = Guid.NewGuid(), Title = dto.Title, TodoTaskId = dto.TodoTaskId };
            var response = new ToDoStepResponseDTO(created.Id, created.Title, created.IsCompleted, created.TodoTaskId);

            mapperMock.Setup(m => m.Map<ToDoStep>(dto)).Returns(step);
            repoMock.Setup(r => r.CreateToDoStepAsync(step)).ReturnsAsync(created);
            mapperMock.Setup(m => m.Map<ToDoStepResponseDTO>(created)).Returns(response);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.CreateToDoStepAsync(dto);

            // Assert
            result.Should().BeSameAs(response);
            mapperMock.Verify(m => m.Map<ToDoStep>(dto), Times.Once);
            repoMock.Verify(r => r.CreateToDoStepAsync(step), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStepResponseDTO>(created), Times.Once);
        }

        [Fact]
        public async Task GetToDoStepByIdAsync_WhenNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var id = Guid.NewGuid();
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync((ToDoStep)null!);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            Func<Task> act = async () => await service.GetToDoStepByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep by id {id} does not found.");
        }

        [Fact]
        public async Task GetToDoStepByIdAsync_WhenFound_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var id = Guid.NewGuid();
            var step = new ToDoStep { Id = id, Title = "t", TodoTaskId = Guid.NewGuid() };
            var response = new ToDoStepResponseDTO(step.Id, step.Title, step.IsCompleted, step.TodoTaskId);

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(step);
            mapperMock.Setup(m => m.Map<ToDoStepResponseDTO>(step)).Returns(response);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.GetToDoStepByIdAsync(id);

            // Assert
            result.Should().BeSameAs(response);
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStepResponseDTO>(step), Times.Once);
        }

        [Fact]
        public async Task GetToDoStepsByTaskIdAsync_ReturnsMappedCollection()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var taskId = Guid.NewGuid();

            var steps = new List<ToDoStep>
            {
                new ToDoStep { Id = Guid.NewGuid(), Title = "a", TodoTaskId = taskId },
                new ToDoStep { Id = Guid.NewGuid(), Title = "b", TodoTaskId = taskId }
            };

            var responses = new List<ToDoStepResponseDTO>
            {
                new ToDoStepResponseDTO(steps[0].Id, steps[0].Title, steps[0].IsCompleted, steps[0].TodoTaskId),
                new ToDoStepResponseDTO(steps[1].Id, steps[1].Title, steps[1].IsCompleted, steps[1].TodoTaskId)
            };

            repoMock.Setup(r => r.GetToDoStepsByTaskIdAsync(taskId)).ReturnsAsync(steps);
            mapperMock.Setup(m => m.Map<IEnumerable<ToDoStepResponseDTO>>(steps)).Returns(responses);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.GetToDoStepsByTaskIdAsync(taskId);

            // Assert
            result.Should().BeSameAs(responses);
            repoMock.Verify(r => r.GetToDoStepsByTaskIdAsync(taskId), Times.Once);
            mapperMock.Verify(m => m.Map<IEnumerable<ToDoStepResponseDTO>>(steps), Times.Once);
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WithEmptyTitle_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            var dto = new ToDoStepUpdateDTO(Guid.NewGuid(), " ", false, Guid.NewGuid());

            // Act
            Func<Task> act = async () => await service.UpdateToDoStepAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Step name cannot be empty.");
            repoMock.VerifyNoOtherCalls();
            mapperMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WhenExistingNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var id = Guid.NewGuid();
            var dto = new ToDoStepUpdateDTO(id, "title", false, Guid.NewGuid());
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync((ToDoStep)null!);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            Func<Task> act = async () => await service.UpdateToDoStepAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep by id {id} does not found to update.");
        }

        [Fact]
        public async Task UpdateToDoStepAsync_WithValidDto_ReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();

            var id = Guid.NewGuid();
            var dto = new ToDoStepUpdateDTO(id, "new title", true, Guid.NewGuid());

            var existing = new ToDoStep { Id = id, Title = "old", TodoTaskId = dto.TodoTaskId };
            var mapped = new ToDoStep { Id = id, Title = dto.Title, IsCompleted = dto.IsCompleted, TodoTaskId = dto.TodoTaskId };
            var updated = new ToDoStep { Id = id, Title = dto.Title, IsCompleted = dto.IsCompleted, TodoTaskId = dto.TodoTaskId };
            var response = new ToDoStepResponseDTO(updated.Id, updated.Title, updated.IsCompleted, updated.TodoTaskId);

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(existing);
            mapperMock.Setup(m => m.Map<ToDoStep>(dto)).Returns(mapped);
            repoMock.Setup(r => r.UpdateToDoStepAsync(mapped)).ReturnsAsync(updated);
            mapperMock.Setup(m => m.Map<ToDoStepResponseDTO>(updated)).Returns(response);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.UpdateToDoStepAsync(dto);

            // Assert
            result.Should().BeSameAs(response);
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStep>(dto), Times.Once);
            repoMock.Verify(r => r.UpdateToDoStepAsync(mapped), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStepResponseDTO>(updated), Times.Once);
        }
        [Fact]
        public async Task DeleteToDoStepAsync_WhenNotFound_ThrowsException()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();
            var id = Guid.NewGuid();
            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync((ToDoStep)null!);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            Func<Task> act = async () => await service.DeleteToDoStepAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"ToDoStep by id {id} does not found.");
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            repoMock.VerifyNoOtherCalls();
            mapperMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task DeleteToDoStepAsync_WhenFound_DeletesAndReturnsMappedResponse()
        {
            // Arrange
            var repoMock = new Mock<IToDoStepRepository>();
            var mapperMock = new Mock<IMapper>();

            var id = Guid.NewGuid();
            var step = new ToDoStep { Id = id, Title = "to delete", TodoTaskId = Guid.NewGuid() };
            var deleted = new ToDoStep { Id = id, Title = "to delete", TodoTaskId = step.TodoTaskId };
            var response = new ToDoStepResponseDTO(deleted.Id, deleted.Title, deleted.IsCompleted, deleted.TodoTaskId);

            repoMock.Setup(r => r.GetToDoStepByIdAsync(id)).ReturnsAsync(step);
            repoMock.Setup(r => r.DeleteToDoStepAsync(step)).ReturnsAsync(deleted);
            mapperMock.Setup(m => m.Map<ToDoStepResponseDTO>(deleted)).Returns(response);

            var service = new ToDoStepService(repoMock.Object, mapperMock.Object);

            // Act
            var result = await service.DeleteToDoStepAsync(id);

            // Assert
            result.Should().BeSameAs(response);
            repoMock.Verify(r => r.GetToDoStepByIdAsync(id), Times.Once);
            repoMock.Verify(r => r.DeleteToDoStepAsync(step), Times.Once);
            mapperMock.Verify(m => m.Map<ToDoStepResponseDTO>(deleted), Times.Once);
        }


    }
}
