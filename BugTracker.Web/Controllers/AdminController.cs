using System;
using System.Linq;
using System.Web.Mvc;
using BugTracker.Data.Context;
using BugTracker.Core.Interfaces;
using System.Data.Entity;

namespace BugTracker.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserContext _userContext;
        public AdminController(IUserContext userContext) { _userContext = userContext; }

        public ActionResult Roles()
        {
            using (var db = new BugTrackerDbContext(_userContext))
            {
                var roles = db.Roles
                    .Include(r => r.RolePermissions.Select(rp => rp.Permission))
                    .ToList();
                return View(roles);
            }
        }

        [HttpPost]
        public ActionResult UpdatePermission(int roleId, int permissionId, string column, bool value)
        {
            try
            {
                using (var db = new BugTrackerDbContext(_userContext))
                {
                    var rp = db.RolePermissions.FirstOrDefault(x => x.RoleId == roleId && x.PermissionId == permissionId);
                    if (rp == null) return Json(new { success = false, message = "Not found" });

                    switch (column)
                    {
                        case "CanOpen": rp.CanOpen = value; break;
                        case "CanInProgress": rp.CanInProgress = value; break;
                        case "CanFixed": rp.CanFixed = value; break;
                        case "CanClosed": rp.CanClosed = value; break;
                    }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }
    }
}