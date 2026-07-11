using Application.Services.Interfaces;
using Domain.Interfaces;
using System.Reflection;

namespace Application.Services
{

    public class EntityOwnershipService : IEntityOwnershipService
    {

        private IModelRepository ModelRepository { get; init; }


        public EntityOwnershipService(IModelRepository modelRepository)
        {
            ModelRepository = modelRepository;
        }


        public async Task<bool> IsUserOwnerAsync(Guid userId, Guid entityId, string entityName)
        {
            IEnumerable<Type?> entityTypes = ModelRepository.GetEntityTypes();
            Type? entityType;

            entityType = entityTypes.FirstOrDefault(e =>
            (
                e is not null ?
                (e.Name.ToLower() == entityName.ToLower())
                : false)
            );

            if (entityType is null)
            {
                throw new ArgumentException($"Entity with name '{entityName}' does not exist in the current context.");
            }

            object? entity = await ModelRepository.FindEntityAsync(entityType, entityId);

            if (entity is null)
            {
                throw new ArgumentException($"Entity with name '{entityName}' and ID '{entityId}' does not exist.");
            }

            if (entity.GetType().GetProperty("AuthorId") is not PropertyInfo AuthorId)
            {
                throw new ArgumentException($"Entity with name '{entityName}' does not have an 'AuthorId' property.");
            }

            if (AuthorId.GetValue(entity) is Guid authorId && authorId == userId)
            {
                return true;
            }

            return false;
        }
    }
}
