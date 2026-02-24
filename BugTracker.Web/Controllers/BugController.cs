using System.Linq;
using System.Web.Mvc;
using BugTracker.Core.DTOs;
using BugTracker.Core.Enums;
using BugTracker.Core.Interfaces;
using BugTracker.Core.ViewModels;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace BugTracker.Web.Controllers
{
    [Authorize]
    public class BugController : Controller
    {
        private readonly IBugService _bugService;
        private readonly IUserPermissionManager _permManager; 

        public BugController(IBugService bugService, IUserPermissionManager permManager)
        {
            _bugService = bugService;
            _permManager = permManager;
        }

        [HttpGet]
        public ActionResult Index(BugFilterDto filter)
        {

            filter = filter ?? new BugFilterDto { Page = 1 };
            var dashboardData = _bugService.GetDashboard(filter);

            var model = new BugDashboardViewModel
            {
                Bugs = dashboardData.Bugs.Select(b => new BugRowViewModel
                {
                    Bug = b,
                    CanOpen = _bugService.CanChangeStatusFlag(BugStatus.Open),
                    CanInProgress = _bugService.CanChangeStatusFlag(BugStatus.InProgress),
                    CanFixed = _bugService.CanChangeStatusFlag(BugStatus.Fixed),
                    CanClosed = _bugService.CanChangeStatusFlag(BugStatus.Closed)
                }).ToList(),

                Users = _bugService.GetUserPermissions(),
                BugGroups = dashboardData.BugGroups,
                TotalBugs = dashboardData.TotalBugs,
                OpenBugs = dashboardData.OpenBugs,
                InProgressBugs = dashboardData.InProgressBugs,
                ClosedBugs = dashboardData.ClosedBugs,

                CurrentPage = filter.Page
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateUserRole(int userId, string roleType = null, bool cO = false, bool cI = false, bool cF = false, bool cC = false)
        {

            try
            {
                var manager = _permManager.ForUser(userId);

                if (!string.IsNullOrEmpty(roleType))
                {
                    if (roleType == "Admin") manager.SetAdmin();
                    else if (roleType == "Dev") manager.SetDev();
                    else if (roleType == "Tester") manager.SetTester();
                }
                else
                {
                    manager.SetRole(cO, cI, cF, cC);
                }

                manager.Save();
                return Json(new { success = true, message = "Cập nhật quyền thành công cho User " + userId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Create(CreateBugDto dto)
        {
            var result = _bugService.Create(dto, GetCurrentUserId());
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public ActionResult Edit(EditBugDto dto)
        {
            var result = _bugService.Edit(dto, GetCurrentUserId());
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, BugStatus status)
        {
            var result = _bugService.ChangeStatus(id, status);
            return Json(new { success = result.Success, message = result.Message });
        }

        private int GetCurrentUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}