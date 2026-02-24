using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using BugTracker.Core.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using Microsoft.Owin.Security;
using System.Linq;
using System.Data.Entity;
public class WebUserContext : IUserContext
{
    private IAuthenticationManager Auth =>
        HttpContext.Current.GetOwinContext().Authentication;
    public int? UserId
    {
        get
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var claim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : (int?)null;
        }
    }

    public string UserName =>
        HttpContext.Current.User.Identity?.Name;

    public string IP
    {
        get
        {
            var request = HttpContext.Current?.Request;
            if (request == null)
                return null;
            var forwarded = request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }
            var remoteIp = request.ServerVariables["REMOTE_ADDR"];
            if (!string.IsNullOrWhiteSpace(remoteIp))
                return remoteIp;

            return request.UserHostAddress;
        }
    }

    public void SignIn(int userId, string userName)
    {
        var permissions = new[]
        {
        "Bug.View",
        "Bug.Create",
        "Bug.Assign",
        "Bug.Close",
        "User.Manage"
    };

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Name, userName),
        new Claim("UserId", userId.ToString())
    };

        foreach (var p in permissions)
        {
            claims.Add(new Claim("permission", p));
        }

        var identity = new ClaimsIdentity(
            claims,
            "ApplicationCookie"
        );

        Auth.SignOut("ApplicationCookie");
        Auth.SignIn(
            new AuthenticationProperties { IsPersistent = true },
            identity
        );
    }

    public void SignOut()
    {
        Auth.SignOut("ApplicationCookie");
    }
}