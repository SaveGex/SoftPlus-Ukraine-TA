using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto);
    }
}
