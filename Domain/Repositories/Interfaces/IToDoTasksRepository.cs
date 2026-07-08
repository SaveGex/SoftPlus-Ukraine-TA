using Domain.Models;

namespace Domain.Repositories.Interfaces
{
    public interface IToDoTaskRepository
    {
        Task<ToDoTask> CreateToDoTaskAsync(ToDoTask task);
        Task<ToDoTask> GetToDoTaskByIdAsync(Guid taskId);
        Task<ToDoTask?> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId);
        Task<IEnumerable<ToDoTask>> GetAllToDoTasksAsync();
        Task<IEnumerable<ToDoTask>> GetMyDayToDoTasksAsync();
        Task<ToDoTask> UpdateToDoTaskAsync(ToDoTask task);
        Task<ToDoTask> DeleteToDoTaskAsync(ToDoTask task);
    }
}
