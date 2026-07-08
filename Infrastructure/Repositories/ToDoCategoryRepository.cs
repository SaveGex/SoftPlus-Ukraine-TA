using Domain.Models;
using Domain.Repositories.Interfaces;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ToDoCategoryRepository : IToDoCategoryRepository
    {
        private readonly ToDoDBContext _context;

        public ToDoCategoryRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public async Task<ToDoCategory> CreateToDoCategoryAsync(ToDoCategory category)
        {
            await _context.ToDoCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ToDoCategory> GetToDoCategoryByIdAsync(Guid categoryId)
        {
            ToDoCategory? category = await _context.ToDoCategories
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync();

            if (category is null)
            {
                throw new Exception("ToDoCategory not found by this Id...");
            }

            return category;
        }

        public async Task<ToDoCategory?> GetToDoCategoryByIdIncludeTasksAsync(Guid categoryId)
        {
            return await _context.ToDoCategories
                .Include(c => c.Tasks)
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ToDoCategory>> GetAllToDoCategoriesAsync()
        {
            return await _context.ToDoCategories.ToListAsync();
        }

        public async Task<ToDoCategory> UpdateToDoCategoryAsync(ToDoCategory category)
        {
            _context.ToDoCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ToDoCategory> DeleteToDoCategoryAsync(ToDoCategory category)
        {
            _context.ToDoCategories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
