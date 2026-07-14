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

        public async Task LoadReferenceAsync(object entity, string navigationPropertyName)
        {
            var reference = _context.Entry(entity).Reference(navigationPropertyName);
            if (!reference.IsLoaded) // just in case if the global query load something...
                await reference.LoadAsync();
        }

    }
}
