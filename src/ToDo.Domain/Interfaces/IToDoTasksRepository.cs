using Domain.Models;

namespace Domain.Interfaces
{
    public interface IToDoTaskRepository
    {
        Task<ToDoTask> CreateToDoTaskAsync(ToDoTask task);
        Task<ToDoTask> GetToDoTaskByIdAsync(Guid taskId, Guid ownerId);
        Task<ToDoTask?> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId, Guid ownerId);
        Task<IEnumerable<ToDoTask>> GetAllToDoTasksAsync(Guid ownerId);
        Task<IEnumerable<ToDoTask>> GetMyDayToDoTasksAsync(Guid ownerId);
        Task<ToDoTask> UpdateToDoTaskAsync(ToDoTask task);
        Task<ToDoTask> DeleteToDoTaskAsync(ToDoTask task);
    }
}
