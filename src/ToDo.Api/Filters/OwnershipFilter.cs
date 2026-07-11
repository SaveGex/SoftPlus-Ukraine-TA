using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace To_Do_application.Filters
{
    public class OwnershipFilter : IAsyncAuthorizationFilter
    {
        private readonly IEntityOwnershipService _ownershipService;
        private readonly string _entityIdKey;
        private readonly string _entityName;

        /// <summary>
        /// Specifies that a user ownership of an entity or it's have special permission (admin, manager)
        /// </summary>
        /// <param name="entityIdKey">The key representing a <see cref="Guid"/> variable from ROUTE which is contains Id of an entity</param>
        /// <param name="entityName">The name in plural of the entity to which the filter applies. <br/><b>For instance <see cref="User"/> - Users</b></param>
        public OwnershipFilter(IEntityOwnershipService _ownershipService, string entityIdKey, string entityName)
        {
            this._ownershipService = _ownershipService;
            _entityIdKey = entityIdKey;
            _entityName = entityName;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Any(c => c.Value == RoleNames.Admin.ToString() || c.Value == RoleNames.Manager.ToString()))
            //{
            //    return;
            //}
            var routeData = context.RouteData.Values;

            if (!routeData.TryGetValue(_entityIdKey, out var idObj) || !Guid.TryParse(idObj?.ToString(), out Guid entityId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            bool isOwner = await _ownershipService.IsUserOwnerAsync(userId, entityId, _entityName);
            if (!isOwner)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
