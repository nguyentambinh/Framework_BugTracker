using System;
using System.Linq;
using System.Web.Mvc;
using BugTracker.Core.DTOs;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.Interfaces;
using BugTracker.Service.Services;

namespace BugTracker.Web.Controllers
{
    public class BugController : Controller
    {
        private readonly IBugService _bugService;

        public BugController()
        {
            _bugService = new BugService();
        }

        public ActionResult Index(BugStatus? status, int? groupId, int? userId, int page = 1)
        {
            var bugs = _bugService.GetAll();

            // LỌC
            if (status.HasValue)
                bugs = bugs.Where(x => x.Status == status.Value);

            if (groupId.HasValue)
                bugs = bugs.Where(x => x.BugGroupId == groupId.Value);

            if (userId.HasValue)
                bugs = bugs.Where(x => x.AssignedToUserId == userId.Value);

            var bugList = bugs.ToList();

            var vm = new BugDashboardViewModel
            {
                Bugs = bugList,

                BugGroups = _bugService.GetBugGroups().ToList(),
                Users = _bugService.GetUsers().ToList(),

                TotalBugs = bugList.Count,
                OpenBugs = bugList.Count(x => x.Status == BugStatus.Open),
                InProgressBugs = bugList.Count(x => x.Status == BugStatus.InProgress),
                ClosedBugs = bugList.Count(x => x.Status == BugStatus.Closed)
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
            _bugService.ChangeStatus(id, BugStatus.Closed);
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