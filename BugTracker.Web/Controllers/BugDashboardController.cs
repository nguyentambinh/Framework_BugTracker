using BugTracker.Core.Enums;
using BugTracker.Service.Services;
using System;
using System.Linq;
using System.Web.Mvc;

public class BugDashboardController : Controller
{
    private readonly BugService _bugService;

    public BugDashboardController()
    {
        _bugService = new BugService();
    }

    public ActionResult Index(int page = 1)
    {
        const int pageSize = 12;

        var pagedResult = _bugService.GetPaged(page, pageSize);

        var vm = new BugDashboardViewModel
        {
            Bugs = pagedResult.Items.ToList(),

            TotalBugs = pagedResult.TotalCount,
            OpenBugs = _bugService.CountByStatus(BugStatus.Open),
            InProgressBugs = _bugService.CountByStatus(BugStatus.InProgress),
            ClosedBugs = _bugService.CountByStatus(BugStatus.Closed),

            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(
                pagedResult.TotalCount / (double)pageSize
            )
        };

        return View(vm);
    }
}