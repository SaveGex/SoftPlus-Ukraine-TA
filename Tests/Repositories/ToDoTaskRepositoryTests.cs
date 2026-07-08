using Domain.Models;
using Infrastructure.DB;
using Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;

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


    [Fact]
    public async Task GetToDoTaskByIdAsync_TaskExists_ReturnsTask()
    {
        // Arrange
        var task = new ToDoTask { Title = "Findable Task" };
        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetToDoTaskByIdAsync(task.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(task.Id);
        result.Title.Should().Be("Findable Task");
    }

    [Fact]
    public async Task GetToDoTaskByIdAsync_TaskDoesNotExist_ThrowsException()
    {
        // Arrange
        var missingId = Guid.NewGuid();

        // Act
        Func<Task> act = () => _sut.GetToDoTaskByIdAsync(missingId);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("ToDoTask not found by this Id...");
    }

    [Fact]
    public async Task GetAllToDoTasksAsync_WhenTasksExist_ReturnsAllTasks()
    {
        // Arrange
        var task1 = new ToDoTask { Title = "T1" };
        var task2 = new ToDoTask { Title = "T2" };
        _context.ToDoTasks.AddRange(task1, task2);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetAllToDoTasksAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(t => t.Title).Should().Contain(new[] { "T1", "T2" });
    }

    [Fact]
    public async Task GetMyDayToDoTasksAsync_FiltersCorrectly_ReturnsOnlyMyDayUncompleted()
    {
        // Arrange
        var included = new ToDoTask { Title = "Include", IsMyDay = true, IsCompleted = false };
        var completed = new ToDoTask { Title = "Completed", IsMyDay = true, IsCompleted = true };
        var notMyDay = new ToDoTask { Title = "NotMyDay", IsMyDay = false, IsCompleted = false };

        _context.ToDoTasks.AddRange(included, completed, notMyDay);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetMyDayToDoTasksAsync();

        // Assert
        result.Should().ContainSingle()
            .Which.Title.Should().Be("Include");
    }

    [Fact]
    public async Task UpdateToDoTaskAsync_ShouldUpdateAndReturnTask()
    {
        // Arrange
        var task = new ToDoTask { Title = "Old Title" };
        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Modify
        task.Title = "New Title";

        // Act
        var returned = await _sut.UpdateToDoTaskAsync(task);

        // Assert
        returned.Should().NotBeNull();
        returned.Id.Should().Be(task.Id);
        returned.Title.Should().Be("New Title");

        _context.ChangeTracker.Clear();
        var dbTask = await _context.ToDoTasks.FindAsync(task.Id);
        dbTask.Should().NotBeNull();
        dbTask!.Title.Should().Be("New Title");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
