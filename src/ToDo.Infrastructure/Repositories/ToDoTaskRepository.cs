using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    internal class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly ToDoDBContext _context;
        public ToDoTaskRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public async Task<ToDoTask> CreateToDoTaskAsync(ToDoTask task)
        {
            await _context.ToDoTasks.AddAsync(task);

            return task;
        }

        public async Task<ToDoTask> GetToDoTaskByIdAsync(Guid taskId, Guid ownerId)
        {
            ToDoTask? task = await _context.ToDoTasks
                .Where(t => t.Id == taskId && t.AuthorId == ownerId)
                .SingleOrDefaultAsync();

            if (task is null)
            {
                throw new Exception("ToDoTask not found by this Id...");
            }

            return task;
        }

        public async Task<ToDoTask?> GetToDoTaskByIdIncludeStepsAndCategoryAsync(Guid taskId, Guid ownerId)
        {
            return await _context.ToDoTasks
                .Include(t => t.Steps)
                .Include(t => t.Category)
                .Where(t => t.AuthorId == ownerId)
                .Where(t => t.Id == taskId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetAllToDoTasksAsync(Guid ownerId)
        {
            return await _context.ToDoTasks
                .Where(t => t.AuthorId == ownerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetMyDayToDoTasksAsync(Guid ownerId)
        {
            return await _context.ToDoTasks
                .Where(t => t.AuthorId == ownerId)
                .Where(t => t.IsMyDay && !t.IsCompleted)
                .ToListAsync();
        }

        public async Task<ToDoTask> UpdateToDoTaskAsync(ToDoTask task)
        {
            _context.ToDoTasks.Update(task);

            return task;
        }

        public async Task<ToDoTask> DeleteToDoTaskAsync(ToDoTask task)
        {
            _context.ToDoTasks.Remove(task);

            return task;
        }
    }
}
