using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IToDoTaskService
    {
        Task<ToDoTaskResponseDTO> CreateToDoTaskAsync(ToDoTaskCreateDTO dto);
        Task<ToDoTaskResponseDTO> GetToDoTaskByIdAsync(Guid taskId);
        Task<ToDoTaskResponseDTO> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId);
        Task<IEnumerable<ToDoTaskResponseDTO>> GetModelAllToDoTasksAsync();
        Task<IEnumerable<ToDoTaskResponseDTO>> GetMyDayToDoTasksAsync();
        Task<ToDoTaskResponseDTO> UpdateToDoTaskAsync(ToDoTaskUpdateDTO dto);
        Task<ToDoTaskResponseDTO> DeleteToDoTaskAsync(Guid taskId);
    }
}
