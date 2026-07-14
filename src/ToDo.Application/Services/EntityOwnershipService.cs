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


        public async Task<bool> IsUserOwnerAsync(Guid userId, Guid entityId, string authorPath)
        {
            var segments = authorPath.Split('.');

            var rootEntityType = ModelRepository.GetEntityTypes()
                .FirstOrDefault(t => t?.Name == segments[0])
                ?? throw new ArgumentException($"Unknown entity type '{segments[0]}' in ownership path '{authorPath}'.");

            object? current = await ModelRepository.FindEntityAsync(rootEntityType, entityId)
                ?? throw new ArgumentException($"Entity '{segments[0]}' with ID '{entityId}' does not exist.");

            for (int i = 1; i < segments.Length - 1; i++)
            {
                var navPropName = segments[i];

                await ModelRepository.LoadReferenceAsync(current, navPropName);

                var navProp = current.GetType().GetProperty(navPropName)
                    ?? throw new ArgumentException(
                        $"Type '{current.GetType().Name}' has no navigation property '{navPropName}' (path: '{authorPath}').");

                current = navProp.GetValue(current)
                    ?? throw new ArgumentException(
                        $"Navigation '{navPropName}' resolved to null while checking ownership (path: '{authorPath}').");
            }

            var authorPropName = segments[^1];
            var authorProp = current.GetType().GetProperty(authorPropName)
                ?? throw new ArgumentException(
                    $"Type '{current.GetType().Name}' has no property '{authorPropName}' (path: '{authorPath}').");

            if (authorProp.GetValue(current) is not Guid authorId)
                throw new ArgumentException(
                    $"Property '{authorPropName}' on '{current.GetType().Name}' is not a Guid (path: '{authorPath}').");

            return authorId == userId;
        }
    }
}
