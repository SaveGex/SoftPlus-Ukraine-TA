using Application.DTOs;
using Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Services
{
    public class AuthServiceTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IAuthService _sut;

        public AuthServiceTests(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _sut = scope.ServiceProvider.GetRequiredService<IAuthService>();
        }

        [Fact]
        public async Task RegisterAndLogin_ShouldAuthenticateSuccessfully_WhenCredentialsAreValid()
        {
            var email = $"user_{Guid.NewGuid()}@example.com";
            var password = "Password123!";

            var registerDto = new RegisterRequestDTO(email, password);
            var registerResult = await _sut.RegisterAsync(registerDto);
            registerResult.Should().NotBeNull();

            var loginDto = new LoginRequestDTO(email, password);
            var loginResult = await _sut.LoginAsync(loginDto);

            loginResult.Should().NotBeNull();
            loginResult.IsSuccess.Should().BeTrue();
            loginResult.Token.Should().NotBeNullOrEmpty();
        }
    }
}
