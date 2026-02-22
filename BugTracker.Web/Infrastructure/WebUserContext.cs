using System.Web;
using BugTracker.Core.Interfaces;

namespace BugTracker.Web.Infrastructure
{
    public class WebUserContext : IUserContext
    {
        public int? UserId =>
            HttpContext.Current?.Session?["UserId"] as int?;

        public string UserName =>
            HttpContext.Current?.User?.Identity?.Name ?? "System";

        public string IpAddress =>
            HttpContext.Current?.Request?.UserHostAddress;
    }
}