using Domain.Models;
using Domain.Repositories.Interfaces;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ToDoStepRepository : IToDoStepRepository
    {
        private readonly ToDoDBContext _context;

        public ToDoStepRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public async Task<ToDoStep> CreateToDoStepAsync(ToDoStep step)
        {
            await _context.ToDoSteps.AddAsync(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task<ToDoStep> GetToDoStepByIdAsync(Guid stepId)
        {
            ToDoStep? step = await _context.ToDoSteps
                .Where(s => s.Id == stepId)
                .SingleOrDefaultAsync();

            if (step is null)
            {
                throw new Exception("ToDoStep not found by this Id...");
            }

            return step;
        }

        public async Task<IEnumerable<ToDoStep>> GetToDoStepsByTaskIdAsync(Guid taskId)
        {
            return await _context.ToDoSteps
                .Where(s => s.TodoTaskId == taskId)
                .ToListAsync();
        }

        public async Task<ToDoStep> UpdateToDoStepAsync(ToDoStep step)
        {
            _context.ToDoSteps.Update(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task<ToDoStep> DeleteToDoStepAsync(ToDoStep step)
        {
            _context.ToDoSteps.Remove(step);
            await _context.SaveChangesAsync();
            return step;
        }
    }
}
