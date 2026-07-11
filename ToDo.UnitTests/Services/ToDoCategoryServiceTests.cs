using Application.DTOs;
using Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Services
{
    public class ToDoCategoryServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IToDoCategoryService _sut;

        public ToDoCategoryServiceTests(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _sut = scope.ServiceProvider.GetRequiredService<IToDoCategoryService>();
        }

        [Fact]
        public async Task CreateAndGetCategory_ShouldPersistAndRetrieveCorrectly()
        {
            var createDto = new ToDoCategoryCreateDTO("Work Category", null);

            var createdCategory = await _sut.CreateToDoCategoryAsync(createDto);
            createdCategory.Should().NotBeNull();

            var retrievedCategory = await _sut.GetToDoCategoryByIdAsync(createdCategory.Id);

            retrievedCategory.Should().NotBeNull();
            retrievedCategory.Id.Should().Be(createdCategory.Id);
            retrievedCategory.Name.Should().Be("Work Category");
        }
    }
}
