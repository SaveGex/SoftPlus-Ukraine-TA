using Domain.Interfaces;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    internal class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        protected readonly ToDoDBContext _context;

        public UnitOfWorkRepository(ToDoDBContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
