using Microsoft.AspNetCore.Mvc;

namespace To_Do_application.Filters.FilterAttributes
{
    public class OwnershipAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Specifies that a user ownership of an entity or it's have special permission (admin, manager)
        /// </summary>
        /// <param name="entityIdKey">The key representing a <see cref="Guid"/> variable from route which is contains Id of an entity</param>
        /// <param name="entityName">The name in plural of the entity to which the filter applies. <br/><b>For instance <see cref="User"/> - Users</b></param>
        public OwnershipAttribute(string entityIdKey, string entityName)
            : base(typeof(OwnershipFilter))
        {
            Arguments = new object[] { entityIdKey, entityName };
        }
    }
}
