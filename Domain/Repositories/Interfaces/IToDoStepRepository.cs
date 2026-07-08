using Domain.Models;

namespace Domain.Repositories.Interfaces
{
    public interface IToDoStepRepository
    {
        Task<ToDoStep> CreateToDoStepAsync(ToDoStep step);
        Task<ToDoStep> GetToDoStepByIdAsync(Guid stepId);
        Task<IEnumerable<ToDoStep>> GetToDoStepsByTaskIdAsync(Guid taskId);
        Task<ToDoStep> UpdateToDoStepAsync(ToDoStep step);
        Task<ToDoStep> DeleteToDoStepAsync(ToDoStep step);
    }
}
