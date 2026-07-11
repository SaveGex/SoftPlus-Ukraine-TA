using Domain.Models;

namespace Domain.Interfaces
{
    public interface IToDoCategoryRepository
    {
        Task<ToDoCategory> CreateToDoCategoryAsync(ToDoCategory category);
        Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid categoryId, Guid ownerId);
        Task<ToDoCategory?> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId, Guid ownerId);
        Task<IEnumerable<ToDoCategory>> GetAllToDoCategoriesAsync(Guid ownerId);
        Task<ToDoCategory> UpdateToDoCategoryAsync(ToDoCategory category);
        Task<ToDoCategory> DeleteToDoCategoryAsync(ToDoCategory category);
    }
}
