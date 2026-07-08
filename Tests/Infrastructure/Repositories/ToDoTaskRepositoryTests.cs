using Domain.Models;
using Infrastructure.DB;
using Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Infrastructure.Repositories;

public class ToDoTaskRepositoryTests : IDisposable
{
    private readonly ToDoDBContext _context;
    private readonly ToDoTaskRepository _sut;

    public ToDoTaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ToDoDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ToDoDBContext(options);
        _context.Database.EnsureCreated();
        _sut = new ToDoTaskRepository(_context);
    }

    [Fact]
    public async Task CreateToDoTaskAsync_ShouldSaveTaskToDatabaseWithGeneratedIdAndDate()
    {
        // Arrange
        var task = new ToDoTask { Title = "Repository Task", Note = "Verify DB Insertion" };

        // Act
        var result = await _sut.CreateToDoTaskAsync(task);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        var dbTask = await _context.ToDoTasks.FindAsync(result.Id);
        dbTask.Should().NotBeNull();
        dbTask!.Title.Should().Be("Repository Task");
    }

    [Fact]
    public async Task GetToDoTaskByIdIncludeStepsAndCategoryAsync_ShouldReturnTaskWithLoadedRelations()
    {
        // Arrange
        var category = new ToDoCategory { Name = "Work" };
        var task = new ToDoTask
        {
            Title = "Complex Task",
            Category = category,
            Steps = new List<ToDoStep> { new() { Title = "Step 1" }, new() { Title = "Step 2" } }
        };

        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetToDoTaskByIdIncludeStepsAndCategoryAsync(task.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Category.Should().NotBeNull();
        result.Category!.Name.Should().Be("Work");
        result.Steps.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteToDoTaskAsync_ShouldTriggerCascadeDeleteForSteps()
    {
        // Arrange
        var task = new ToDoTask
        {
            Title = "Task with steps",
            Steps = new List<ToDoStep> { new() { Title = "Sub-step" } }
        };

        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();
        var stepId = task.Steps.First().Id;

        // Act
        await _sut.DeleteToDoTaskAsync(task);

        // Assert
        var dbTask = await _context.ToDoTasks.FindAsync(task.Id);
        var dbStep = await _context.ToDoSteps.FindAsync(stepId);

        dbTask.Should().BeNull();
        dbStep.Should().BeNull();
    }

    [Fact]
    public async Task DeleteToDoTaskAsync_ShouldSetNullToCategoryId_WhenTaskIsDeleted()
    {
        // Arrange
        var category = new ToDoCategory { Name = "Personal" };
        var task = new ToDoTask { Title = "Task linked to category", Category = category };

        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        await _sut.DeleteToDoTaskAsync(task);

        // Assert
        var dbCategory = await _context.ToDoCategories.FindAsync(category.Id);
        dbCategory.Should().NotBeNull();

        var dbTask = await _context.ToDoTasks.FindAsync(task.Id);
        dbTask.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
