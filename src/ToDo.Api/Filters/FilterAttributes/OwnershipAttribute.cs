using Microsoft.AspNetCore.Mvc;

namespace To_Do_application.Filters.FilterAttributes
{
    public class OwnershipAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Specifies that a user ownership of an entity or it's have special permission (admin, manager)
        /// </summary>
        /// <param name="entityIdKey">The key representing a <see cref="Guid"/> variable from route which is contains Id of an entity</param>
        /// <param name="authorPath">The dot-separated path to the author property</param>
        public OwnershipAttribute(string entityIdKey, string authorPath)
            : base(typeof(OwnershipFilter))
        {
            Arguments = new object[] { entityIdKey, authorPath };
        }
    }
}
