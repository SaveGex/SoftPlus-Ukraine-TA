using Domain.Interfaces;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    internal class ModelRepository : IModelRepository
    {
        private readonly ToDoDBContext _context;

        public ModelRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Type?> GetEntityTypes()
        {
            return _context.Model.GetEntityTypes()
                .Select(e => e.ClrType);
        }

        public async Task<object?> FindEntityAsync(Type entityType, params object?[]? keyValues)
        {
            return await _context.FindAsync(entityType, keyValues);
        }
    }
}
