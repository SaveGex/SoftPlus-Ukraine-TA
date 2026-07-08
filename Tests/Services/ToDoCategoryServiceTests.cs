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
        private readonly Mock<IMapper> _mapperMock;
        private readonly ToDoCategoryService _sut;

        public ToDoCategoryServiceTests()
        {
            _repositoryMock = new Mock<IToDoCategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new ToDoCategoryService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Constructor_WithDependencies_ShouldCreateInstance()
        {
            // Arrange
            var repo = new Mock<IToDoCategoryRepository>();
            var mapper = new Mock<IMapper>();

            // Act
            var act = () => new ToDoCategoryService(repo.Object, mapper.Object);

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
            var expectedResponse = new ToDoCategoryResponseDTO(domainCategory.Id, domainCategory.Name, domainCategory.Icon, new List<ToDoTaskResponseDTO>());

            _mapperMock.Setup(m => m.Map<ToDoCategory>(createDto)).Returns(domainCategory);
            _repositoryMock.Setup(r => r.CreateToDoCategoryAsync(domainCategory)).ReturnsAsync(domainCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategoryResponseDTO>(domainCategory)).Returns(expectedResponse);

            // Act
            var result = await _sut.CreateToDoCategoryAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedResponse.Id);
            result.Name.Should().Be(expectedResponse.Name);
            _repositoryMock.Verify(r => r.CreateToDoCategoryAsync(domainCategory), Times.Once);
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
            _mapperMock.Verify(m => m.Map<ToDoCategoryResponseDTO>(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_WhenFound_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var domainCategory = new ToDoCategory { Id = id, Name = "Home" };
            var expectedResponse = new ToDoCategoryResponseDTO(id, "Home", null, new List<ToDoTaskResponseDTO>());

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(domainCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategoryResponseDTO>(domainCategory)).Returns(expectedResponse);

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
            _mapperMock.Verify(m => m.Map<ToDoCategoryResponseDTO>(It.IsAny<ToDoCategory>()), Times.Never);
        }

        [Fact]
        public async Task GetToDoCategoryByIdIncludeTasksAsync_WhenFound_ShouldReturnResponseDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var domainCategory = new ToDoCategory { Id = id, Name = "Work" };
            var expectedResponse = new ToDoCategoryResponseDTO(id, "Work", null, new List<ToDoTaskResponseDTO>());

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdIncludeTasksAsync(id)).ReturnsAsync(domainCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategoryResponseDTO>(domainCategory)).Returns(expectedResponse);

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
            var responseList = new List<ToDoCategoryResponseDTO> { new ToDoCategoryResponseDTO(domainList[0].Id, "A", null, new List<ToDoTaskResponseDTO>()) };

            _repositoryMock.Setup(r => r.GetAllToDoCategoriesAsync()).ReturnsAsync(domainList);
            _mapperMock.Setup(m => m.Map<IEnumerable<ToDoCategoryResponseDTO>>(domainList)).Returns(responseList);

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
            var dto = new ToDoCategoryUpdateDTO(Guid.NewGuid(), invalidName, null);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(dto);

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
            var dto = new ToDoCategoryUpdateDTO(Guid.NewGuid(), null!, null);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(dto);

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
            var dto = new ToDoCategoryUpdateDTO(id, "Name", null);
            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync((ToDoCategory?)null!);

            // Act
            var act = () => _sut.UpdateToDoCategoryAsync(dto);

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
            var dto = new ToDoCategoryUpdateDTO(id, "Updated", "icon");
            var existingCategory = new ToDoCategory { Id = id, Name = "Old", Icon = "oldicon" };
            var mappedCategory = new ToDoCategory { Id = id, Name = dto.Name, Icon = dto.Icon };
            var updatedCategory = new ToDoCategory { Id = id, Name = dto.Name, Icon = dto.Icon };
            var expectedResponse = new ToDoCategoryResponseDTO(id, dto.Name, dto.Icon, new List<ToDoTaskResponseDTO>());

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(existingCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategory>(dto)).Returns(mappedCategory);
            _repositoryMock.Setup(r => r.UpdateToDoCategoryAsync(mappedCategory)).ReturnsAsync(updatedCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategoryResponseDTO>(updatedCategory)).Returns(expectedResponse);

            // Act
            var result = await _sut.UpdateToDoCategoryAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedResponse.Id);
            result.Name.Should().Be(expectedResponse.Name);
            _repositoryMock.Verify(r => r.GetToDoCategoryByIdAsync(id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateToDoCategoryAsync(mappedCategory), Times.Once);
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
            var expectedResponse = new ToDoCategoryResponseDTO(id, "ToDelete", null, new List<ToDoTaskResponseDTO>());

            _repositoryMock.Setup(r => r.GetToDoCategoryByIdAsync(id)).ReturnsAsync(domainCategory);
            _repositoryMock.Setup(r => r.DeleteToDoCategoryAsync(domainCategory)).ReturnsAsync(domainCategory);
            _mapperMock.Setup(m => m.Map<ToDoCategoryResponseDTO>(domainCategory)).Returns(expectedResponse);

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
