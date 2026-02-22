using System.Web;
using System.Web.Mvc;

public class HasPermissionAttribute : AuthorizeAttribute
{
    private readonly string _permission;

    public HasPermissionAttribute(string permission)
    {
        _permission = permission;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var permissionService =
            DependencyResolver.Current.GetService<IPermissionService>();

        return permissionService.HasPermission(_permission);
    }
}