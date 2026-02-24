using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Web.Attributes
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        private readonly string _permission;

        public HasPermissionAttribute(string permission)
        {
            _permission = permission;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return false;

            return identity.Claims.Any(c =>
                c.Type == "permission" && c.Value == _permission);
        }
    }
}