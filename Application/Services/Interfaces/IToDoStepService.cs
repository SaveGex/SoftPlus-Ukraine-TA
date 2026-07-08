using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IToDoStepService
    {
        Task<ToDoStepResponseDTO> CreateToDoStepAsync(ToDoStepCreateDTO dto);
        Task<ToDoStepResponseDTO> GetToDoStepByIdAsync(Guid stepId);
        Task<IEnumerable<ToDoStepResponseDTO>> GetToDoStepsByTaskIdAsync(Guid taskId);
        Task<ToDoStepResponseDTO> UpdateToDoStepAsync(ToDoStepUpdateDTO dto);
        Task<ToDoStepResponseDTO> DeleteToDoStepAsync(Guid stepId);
    }
}
