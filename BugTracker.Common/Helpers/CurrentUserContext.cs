using System.Web;

namespace BugTracker.Common.Helpers
{
    public static class CurrentUserContext
    {
        public static int? UserId =>
            HttpContext.Current?.Session?["UserId"] as int?;

        public static string UserName =>
            HttpContext.Current?.User?.Identity?.Name ?? "System";

        public static string Ip =>
            HttpContext.Current?.Request?.UserHostAddress;
    }
}