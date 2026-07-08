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
    public class ToDoCategoryServiceTests
    {
        private readonly Mock<IToDoCategoryRepository> _repositoryMock;
        private readonly ToDoCategoryService _sut;

        public ToDoCategoryServiceTests()
        {
            _repositoryMock = new Mock<IToDoCategoryRepository>();
            _sut = new ToDoCategoryService(_repositoryMock.Object);
        }

        [Fact]
        public void Constructor_WithDependencies_ShouldCreateInstance()
        {
            // Arrange
            var repo = new Mock<IToDoCategoryRepository>();

            // Act
            var act = () => new ToDoCategoryService(repo.Object);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateToDoCategoryAsync_WithInvalidName_ShouldThrowException(string invalidName)
        {
            // Arrange
            var dto = new ToDoCategoryCreateDTO(invalidName, null);

            // Act
            var act = () => _sut.CreateToDoCategoryAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Category name cannot be empty.");
            _repositoryMock.Verify(r => r.CreateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task CreateToDoCategoryAsync_WithNullName_ShouldThrowException()
        {
            // Arrange
            var dto = new ToDoCategoryCreateDTO(null!, null);

            // Act
            var act = () => _sut.CreateToDoCategoryAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Category name cannot be empty.");
            _repositoryMock.Verify(r => r.CreateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task CreateToDoCategoryAsync_WithValidDto_ShouldReturnResponseDto()
        {
            // Arrange
            var createDto = new ToDoCategoryCreateDTO("Groceries", "icon");
            var domainCategory = new ToDoCategory { Id = Guid.NewGuid(), Name = createDto.Name, Icon = createDto.Icon };

            _repositoryMock.Setup(r => r.CreateToDoCategoryAsync(It.IsAny<ToDoCategory>())).ReturnsAsync(domainCategory);

            // Act
            var result = await _sut.CreateToDoCategoryAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(domainCategory.Id);
            result.Name.Should().Be(domainCategory.Name);
            _repositoryMock.Verify(r => r.CreateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Once);
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_WhenNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync((ToDoCategory?)null!);

            // Act
            var act = () => _sut.GetToDoCategoryByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"ToDoCategory by id {id} does not found.");
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_WhenFound_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var domainCategory = new ToDoCategory { Id = id, Name = "Home" };

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(domainCategory);

            // Act
            var result = await _sut.GetToDoCategoryByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Home");
        }

        [Fact]
        public async Task GetToDoCategoryByIdIncludeTasksAsync_WhenNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetToDoCategoryByIdIncludeTasksAsync(id)).ReturnsAsync((ToDoCategory?)null!);

            // Act
            var act = () => _sut.GetToDoCategoryByIdIncludeTasksAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"ToDoCategory by id {id} does not found.");
        }

        [Fact]
        public async Task GetToDoCategoryByIdIncludeTasksAsync_WhenFound_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var domainCategory = new ToDoCategory { Id = id, Name = "Work" };

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdIncludeTasksAsync(id)).ReturnsAsync(domainCategory);

            // Act
            var result = await _sut.GetToDoCategoryByIdIncludeTasksAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Work");
        }

        [Fact]
        public async Task GetAllToDoCategoriesAsync_WhenCategoriesExist_ShouldReturnMappedList()
        {
            // Arrange
            var domainList = new List<ToDoCategory> { new ToDoCategory { Id = Guid.NewGuid(), Name = "A" } };

            _repositoryMock.Setup(r => r.GetAllToDoCategoriesAsync()).ReturnsAsync(domainList);

            // Act
            var result = await _sut.GetAllToDoCategoriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task UpdateToDoCategoryAsync_WithInvalidName_ShouldThrowException(string invalidName)
        {
            // Arrange
            var dto = new ToDoCategoryUpdateDTO(invalidName, null);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(Guid.NewGuid(), dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Category name cannot be empty.");
            _repositoryMock.Verify(r => r.GetToDoCategoryByIdAsync(It.IsAny<Guid>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_WithNullName_ShouldThrowException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var dto = new ToDoCategoryUpdateDTO(null!, null);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(id, dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Category name cannot be empty.");
            _repositoryMock.Verify(r => r.GetToDoCategoryByIdAsync(It.IsAny<Guid>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_WhenCategoryNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ToDoCategoryUpdateDTO("Name", null);
            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync((ToDoCategory?)null!);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(id, dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"ToDoCategory by id {id} does not found to update.");
            _repositoryMock.Verify(r => r.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task UpdateToDoCategoryAsync_WithValidDto_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ToDoCategoryUpdateDTO("Updated", "icon");
            var existingCategory = new ToDoCategory { Id = id, Name = "Old", Icon = "oldicon" };
            var updatedCategory = new ToDoCategory { Id = id, Name = dto.Name, Icon = dto.Icon };

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(existingCategory);
            _repositoryMock.Setup(r => r.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>())).ReturnsAsync(updatedCategory);

            // Act
            var result = await _sut.UpdateToDoCategoryAsync(id, dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(updatedCategory.Id);
            result.Name.Should().Be(updatedCategory.Name);
            _repositoryMock.Verify(r => r.GetToDoCategoryByIdAsync(id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_WhenNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync((ToDoCategory?)null!);

            // Act
            var act = () => _sut.DeleteToDoCategoryAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"ToDoCategory by id {id} does not found.");
            _repositoryMock.Verify(r => r.DeleteToDoCategoryAsync(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_WhenFound_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var domainCategory = new ToDoCategory { Id = id, Name = "ToDelete" };

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(domainCategory);
            _repositoryMock.Setup(r => r.DeleteToDoCategoryAsync(domainCategory)).ReturnsAsync(domainCategory);

            // Act
            var result = await _sut.DeleteToDoCategoryAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("ToDelete");
            _repositoryMock.Verify(r => r.DeleteToDoCategoryAsync(domainCategory), Times.Once);
        }
    }
}
