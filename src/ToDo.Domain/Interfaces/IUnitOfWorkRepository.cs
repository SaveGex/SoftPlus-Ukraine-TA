
namespace Domain.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
