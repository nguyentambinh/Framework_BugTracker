using System.Linq;
using System.Web.Mvc;
using BugTracker.Core.DTOs;
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

        public ActionResult Index(BugStatus? status)
        {
            var bugs = _bugService.GetAll();

            if (status.HasValue)
                bugs = bugs.Where(b => b.Status == status.Value);

            return View(bugs);
        }

        // ================= DROPDOWNS =================
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

        // ================= CREATE =================
        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateBugDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(dto.BugGroupId, dto.AssignedToUserId);
                return View(dto);
            }

            int currentUserId = 2; 
            var result = _bugService.Create(dto, currentUserId);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                LoadDropdowns(dto.BugGroupId, dto.AssignedToUserId);
                return View(dto);
            }

            return RedirectToAction("Index");
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

        public ActionResult Delete(int id)
        {
            _bugService.ChangeStatus(id, BugStatus.Closed);
            return RedirectToAction("Index");
        }
    }
}