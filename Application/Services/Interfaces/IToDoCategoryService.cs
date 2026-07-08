using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IToDoCategoryService
    {
        Task<ToDoCategoryResponseDTO> CreateToDoCategoryAsync(ToDoCategoryCreateDTO dto);
        Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdAsync(Guid categoryId);
        Task<ToDoCategoryResponseDTO> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId);
        Task<IEnumerable<ToDoCategoryResponseDTO>> GetAllToDoCategoriesAsync();
        Task<ToDoCategoryResponseDTO> UpdateToDoCategoryAsync(ToDoCategoryUpdateDTO dto);
        Task<ToDoCategoryResponseDTO> DeleteToDoCategoryAsync(Guid categoryId);
    }
}
