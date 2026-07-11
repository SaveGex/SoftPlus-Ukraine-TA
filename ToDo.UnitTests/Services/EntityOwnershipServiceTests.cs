using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Models;
using FluentAssertions;
using Infrastructure.DB;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Services
{
    public class EntityOwnershipServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IEntityOwnershipService _sut;
        private readonly ToDoDBContext _ctx;

        public EntityOwnershipServiceTests(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _sut = scope.ServiceProvider.GetRequiredService<IEntityOwnershipService>();
            _ctx = scope.ServiceProvider.GetRequiredService<ToDoDBContext>();
        }

        [Fact]
        public async Task IsUserOwnerAsync_ShouldReturnTrue_WhenEntityBelongsToTheUser()
        {
            var currentUserId = Guid.Empty;
            var entityName = nameof(ToDoTask) + "s";

            var dto = new ToDoTaskCreateDTO("Happy Path Task", null, true, true, null, null, Domain.Enums.RecurrenceType.None, null);
            var task = dto.Adapt<ToDoTask>();
            task.Id = Guid.NewGuid();
            task.AuthorId = currentUserId;

            _ctx.ToDoTasks.Add(task);
            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();

            var result = await _sut.IsUserOwnerAsync(currentUserId, task.Id, entityName);

            result.Should().BeTrue();
        }
    }
}
