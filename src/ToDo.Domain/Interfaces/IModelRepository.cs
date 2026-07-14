
namespace Domain.Interfaces
{
    public interface IModelRepository
    {
        IEnumerable<Type?> GetEntityTypes();
        Task<object?> FindEntityAsync(Type clrType, params object?[]? keyValues);
        Task LoadReferenceAsync(object entity, string navigationPropertyName);

    }
}
