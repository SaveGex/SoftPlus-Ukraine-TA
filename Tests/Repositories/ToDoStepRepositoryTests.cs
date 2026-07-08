using Domain.Models;
using Infrastructure.DB;
using Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Repositories;

public class ToDoStepRepositoryTests : IDisposable
{
    private readonly ToDoDBContext _context;
    private readonly ToDoStepRepository _sut;

    public ToDoStepRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ToDoDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ToDoDBContext(options);
        _context.Database.EnsureCreated();
        _sut = new ToDoStepRepository(_context);
    }

    [Fact]
    public void ToDoStepRepository_Constructor_WithValidContext_CreatesInstance()
    {
        // Arrange done in ctor

        // Act & Assert
        _sut.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateToDoStepAsync_WithValidStep_AddsAndReturnsStep()
    {
        // Arrange
        var step = new ToDoStep { Title = "Step 1" };

        // Act
        var result = await _sut.CreateToDoStepAsync(step);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        var dbStep = await _context.ToDoSteps.FindAsync(result.Id);
        dbStep.Should().NotBeNull();
        dbStep!.Title.Should().Be("Step 1");
    }

    [Fact]
    public async Task GetToDoStepByIdAsync_WithExistingStep_ReturnsStep()
    {
        // Arrange
        var step = new ToDoStep { Title = "FindMe" };
        _context.ToDoSteps.Add(step);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetToDoStepByIdAsync(step.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(step.Id);
        result.Title.Should().Be("FindMe");
    }

    [Fact]
    public async Task GetToDoStepByIdAsync_WithMissingStep_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        Func<Task> act = async () => await _sut.GetToDoStepByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("ToDoStep not found by this Id...");
    }

    [Fact]
    public async Task GetToDoStepsByTaskIdAsync_WithMultipleSteps_ReturnsOnlyMatching()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var s1 = new ToDoStep { Title = "A", TodoTaskId = taskId };
        var s2 = new ToDoStep { Title = "B", TodoTaskId = taskId };
        var sOther = new ToDoStep { Title = "X", TodoTaskId = Guid.NewGuid() };

        _context.ToDoSteps.AddRange(s1, s2, sOther);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var result = await _sut.GetToDoStepsByTaskIdAsync(taskId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(s => s.Title).Should().BeEquivalentTo(new[] { "A", "B" });
    }

    [Fact]
    public async Task UpdateToDoStepAsync_WithModifiedStep_PersistsChanges()
    {
        // Arrange
        var step = new ToDoStep { Title = "Old" };
        _context.ToDoSteps.Add(step);
        await _context.SaveChangesAsync();

        // modify
        step.Title = "New";

        // Act
        var result = await _sut.UpdateToDoStepAsync(step);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(step.Id);
        result.Title.Should().Be("New");

        var dbStep = await _context.ToDoSteps.FindAsync(step.Id);
        dbStep.Should().NotBeNull();
        dbStep!.Title.Should().Be("New");
    }

    [Fact]
    public async Task DeleteToDoStepAsync_WithExistingStep_RemovesAndReturnsStep()
    {
        // Arrange
        var step = new ToDoStep { Title = "ToDelete" };
        _context.ToDoSteps.Add(step);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteToDoStepAsync(step);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(step.Id);
        result.Title.Should().Be("ToDelete");

        var dbStep = await _context.ToDoSteps.FindAsync(step.Id);
        dbStep.Should().BeNull();
    }

    [Fact]
    public async Task DeleteToDoStepAsync_WithNonExistingStep_ThrowsDbUpdateConcurrencyException()
    {
        // Arrange
        var step = new ToDoStep { Title = "NotInDb" };

        // Act
        Func<Task> act = async () => await _sut.DeleteToDoStepAsync(step);

        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();

        var all = await _context.ToDoSteps.ToListAsync();
        all.Should().BeEmpty();
    }


    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
