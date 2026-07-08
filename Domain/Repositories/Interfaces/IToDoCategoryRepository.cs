using Domain.Models;

namespace Domain.Repositories.Interfaces
{
    public interface IToDoCategoryRepository
    {
        Task<ToDoCategory> CreateToDoCategoryAsync(ToDoCategory category);
        Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid categoryId);
        Task<ToDoCategory?> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId);
        Task<IEnumerable<ToDoCategory>> GetAllToDoCategoriesAsync();
        Task<ToDoCategory> UpdateToDoCategoryAsync(ToDoCategory category);
        Task<ToDoCategory> DeleteToDoCategoryAsync(ToDoCategory category);
    }
}
