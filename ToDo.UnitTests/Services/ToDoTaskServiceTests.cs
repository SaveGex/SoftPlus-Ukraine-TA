using Application.DTOs;
using Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Services
{
    public class ToDoTaskServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IToDoTaskService _taskService;

        public ToDoTaskServiceTests(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _taskService = scope.ServiceProvider.GetRequiredService<IToDoTaskService>();
        }

        [Fact]
        public async Task CreateAndGetTask_ShouldPersistAndRetrieveCorrectly()
        {
            var createDto = new ToDoTaskCreateDTO("Integration Task", "Description", false, true, null, null, Domain.Enums.RecurrenceType.None, null);

            var createdTask = await _taskService.CreateToDoTaskAsync(createDto);
            createdTask.Should().NotBeNull();

            var retrievedTask = await _taskService.GetToDoTaskByIdAsync(createdTask.Id);

            retrievedTask.Should().NotBeNull();
            retrievedTask.Id.Should().Be(createdTask.Id);
            retrievedTask.Title.Should().Be("Integration Task");
            retrievedTask.IsMyDay.Should().BeTrue();
        }
    }
}
