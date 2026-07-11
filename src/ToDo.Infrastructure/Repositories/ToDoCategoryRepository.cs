using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ToDoCategoryRepository : IToDoCategoryRepository
    {

        private readonly ToDoDBContext _context;
        private IQueryable<ToDoCategory> BaseCategoryQuery =>
            _context.ToDoCategories.Include(c => c.Icon);
        public ToDoCategoryRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public async Task<ToDoCategory> CreateToDoCategoryAsync(ToDoCategory category)
        {
            await _context.ToDoCategories.AddAsync(category);

            return category;
        }

        public async Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid categoryId, Guid ownerId)
        {
            ToDoCategory? category = await BaseCategoryQuery
                .Where(c => c.AuthorId == ownerId)
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync();

            if (category is null)
            {
                throw new Exception("ToDoCategory not found by this Id...");
            }

            return category;
        }

        public async Task<ToDoCategory?> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId, Guid ownerId)
        {
            return await BaseCategoryQuery
                .Include(c => c.Tasks)
                .Where(c => c.AuthorId == ownerId)
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ToDoCategory>> GetAllToDoCategoriesAsync(Guid ownerId)
        {
            return await BaseCategoryQuery
                .Where(c => c.AuthorId == ownerId)
                .ToListAsync();
        }

        public async Task<ToDoCategory> UpdateToDoCategoryAsync(ToDoCategory category)
        {
            _context.ToDoCategories.Update(category);

            return category;
        }

        public async Task<ToDoCategory> DeleteToDoCategoryAsync(ToDoCategory category)
        {
            _context.ToDoCategories.Remove(category);

            return category;
        }
    }
}
