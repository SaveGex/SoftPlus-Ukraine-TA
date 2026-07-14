
namespace Application.Services.Interfaces;

public interface IEntityOwnershipService
{
    Task<bool> IsUserOwnerAsync(Guid userId, Guid entityId, string authorPath);
}
