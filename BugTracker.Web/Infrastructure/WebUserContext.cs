using System.Web;
using BugTracker.Core.Interfaces;

namespace BugTracker.Web.Infrastructure
{
    public class WebUserContext : IUserContext
    {
        public int? UserId =>
            HttpContext.Current?.Session?["UserId"] as int?;

        public string UserName =>
            HttpContext.Current?.Session?["UserName"] as string ?? "System";

        public void SignIn(int userId, string userName)
        {
            var session = HttpContext.Current.Session;
            session["UserId"] = userId;
            session["UserName"] = userName;
        }

        public void SignOut()
        {
            var session = HttpContext.Current.Session;
            session.Remove("UserId");
            session.Remove("UserName");
        }
    }
}