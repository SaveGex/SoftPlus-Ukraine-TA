using Application.DTOs;
using Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Services
{
    public class ToDoStepServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IToDoStepService _stepService;
        private readonly IToDoTaskService _taskService;
        public ToDoStepServiceTests(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _stepService = scope.ServiceProvider.GetRequiredService<IToDoStepService>();
            _taskService = scope.ServiceProvider.GetRequiredService<IToDoTaskService>();
        }

        [Fact]
        public async Task CreateAndGetStep_ShouldPersistAndRetrieveCorrectly()
        {
            var taskDto = new ToDoTaskCreateDTO("Parent Task", null, false, false, null, null, Domain.Enums.RecurrenceType.None, null);
            var parentTask = await _taskService.CreateToDoTaskAsync(taskDto);

            var createStepDto = new ToDoStepCreateDTO("Step One", parentTask.Id);
            var createdStep = await _stepService.CreateToDoStepAsync(createStepDto);
            createdStep.Should().NotBeNull();

            var retrievedStep = await _stepService.GetToDoStepByIdAsync(createdStep.Id);

            retrievedStep.Should().NotBeNull();
            retrievedStep.Id.Should().Be(createdStep.Id);
            retrievedStep.Title.Should().Be("Step One");
        }
    }
}
