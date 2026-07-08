using Domain.Models;
using Infrastructure.DB;
using Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Repositories
{
    public class ToDoCategoryRepositoryTests : IDisposable
    {
        private readonly ToDoDBContext _context;
        private readonly ToDoCategoryRepository _sut;

        public ToDoCategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ToDoDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ToDoDBContext(options);
            _context.Database.EnsureCreated();
            _sut = new ToDoCategoryRepository(_context);
        }

        [Fact]
        public async Task ToDoCategoryRepository_WithValidContext_AllowsOperations()
        {
            // Arrange done in ctor

            // Act
            var all = await _sut.GetAllToDoCategoriesAsync();

            // Assert
            all.Should().NotBeNull();
            all.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateToDoCategoryAsync_ShouldAddCategoryAndReturnIt()
        {
            // Arrange
            var category = new ToDoCategory { Name = "Home", Icon = "house" };

            // Act
            var result = await _sut.CreateToDoCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();

            var dbCategory = await _context.ToDoCategories.FindAsync(result.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be("Home");
            dbCategory.Icon.Should().Be("house");
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldReturnCategory_WhenExists()
        {
            // Arrange
            var category = new ToDoCategory { Name = "Work" };
            _context.ToDoCategories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetToDoCategoryByIdAsync(category.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
        }

        [Fact]
        public async Task GetToDoCategoryByIdAsync_ShouldThrowException_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            Func<Task> act = async () => await _sut.GetToDoCategoryByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("ToDoCategory not found by this Id...");
        }

        [Fact]
        public async Task GetToDoCategoryByIdIncludeTasksAsync_ShouldReturnCategoryWithTasks_WhenExists()
        {
            // Arrange
            var category = new ToDoCategory { Name = "Projects" };
            var task = new ToDoTask { Title = "Proj Task", Category = category };
            category.Tasks.Add(task);

            _context.ToDoCategories.Add(category);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Act
            var result = await _sut.GetToDoCategoryByIdIncludeTasksAsync(category.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
            result.Tasks.Should().NotBeNull();
            result.Tasks.Should().ContainSingle(t => t.Title == "Proj Task");
        }

        [Fact]
        public async Task GetAllToDoCategoriesAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var c1 = new ToDoCategory { Name = "A" };
            var c2 = new ToDoCategory { Name = "B" };
            _context.ToDoCategories.AddRange(c1, c2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetAllToDoCategoriesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Select(c => c.Name).Should().BeEquivalentTo(new[] { "A", "B" });
        }
        [Fact]
        public async Task UpdateToDoCategoryAsync_WithModifiedCategory_ReturnsUpdatedCategoryAndPersists()
        {
            // Arrange
            var category = new ToDoCategory { Name = "Original", Icon = "orig" };
            _context.ToDoCategories.Add(category);
            await _context.SaveChangesAsync();

            // modify
            category.Name = "Updated";
            category.Icon = "upd";

            // Act
            var result = await _sut.UpdateToDoCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);
            result.Name.Should().Be("Updated");
            result.Icon.Should().Be("upd");

            var dbCategory = await _context.ToDoCategories.FindAsync(category.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be("Updated");
            dbCategory.Icon.Should().Be("upd");
        }

        [Fact]
        public async Task DeleteToDoCategoryAsync_WithExistingCategory_RemovesItAndReturnsCategory()
        {
            // Arrange
            var category = new ToDoCategory { Name = "ToDelete" };
            _context.ToDoCategories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.DeleteToDoCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(category.Id);

            var dbCategory = await _context.ToDoCategories.FindAsync(category.Id);
            dbCategory.Should().BeNull();
        }


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
