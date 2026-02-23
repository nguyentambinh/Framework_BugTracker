using System;
using System.Linq;
using System.Web.Mvc;
using BugTracker.Core.DTOs;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.Interfaces;
using BugTracker.Service.Services;
using BugTracker.Web.Infrastructure;

namespace BugTracker.Web.Controllers
{
    public class BugController : Controller
    {
        private readonly BugService _bugService;

        public BugController()
        {
            var userContext = new WebUserContext();
            _bugService = new BugService(userContext);
        }


        public ActionResult Index(BugStatus? status, int? groupId, int? userId, int page = 1)
        {
            var filter = new BugFilterDto
            {
                Status = status,
                GroupId = groupId,
                UserId = userId,
                Page = page
            };

            var dashboard = _bugService.GetDashboard(filter);

            var vm = new BugDashboardViewModel
            {
                Bugs = dashboard.Bugs,
                BugGroups = dashboard.BugGroups,
                Users = dashboard.Users,
                TotalBugs = dashboard.TotalBugs,
                OpenBugs = dashboard.OpenBugs,
                InProgressBugs = dashboard.InProgressBugs,
                ClosedBugs = dashboard.ClosedBugs
            };

            return View(vm);
        }

        private void LoadDropdowns(int? selectedGroupId = null, int? selectedUserId = null)
        {
            ViewBag.BugGroups = new SelectList(
                _bugService.GetBugGroups(),
                "Id",
                "Name",
                selectedGroupId
            );

            ViewBag.Users = new SelectList(
                _bugService.GetUsers(),
                "Id",
                "DisplayName",
                selectedUserId
            );
        }

        public ActionResult Edit(int id)
        {
            var bug = _bugService.GetById(id);
            if (bug == null) return HttpNotFound();

            LoadDropdowns(bug.BugGroupId, bug.AssignedToUserId);
            return View(bug);
        }

        [HttpPost]
        public ActionResult Edit(int id, BugStatus status)
        {
            _bugService.ChangeStatus(id, status);
            return RedirectToAction("Index");
        }

        public ActionResult Closed(int id)
        {
            var result = _bugService.Close(id);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Create(CreateBugDto dto)
        {
            int currentUserId = 2; 
            _bugService.Create(dto, currentUserId);
            return RedirectToAction("Index");
        }
    }
}