using BugTracker.Core.Base;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.DTOs;
using System.Collections.Generic;
using BugTracker.Core.ViewModels;

namespace BugTracker.Core.Interfaces
{
    public interface IBugService
    {
        ServiceResult Create(CreateBugDto dto, int currentUserId);
        ServiceResult Edit(EditBugDto dto, int currentUserId);
        ServiceResult Assign(int bugId, int assignToUserId);
        ServiceResult ChangeStatus(int bugId, BugStatus newStatus);
        bool CanChangeStatusFlag(BugStatus status);
        List<UserPermissionViewModel> GetUserPermissions();
        IEnumerable<Bug> GetAll();
        IEnumerable<Bug> GetOpenBugs();
        PagedResult<Bug> GetPaged(int page, int pageSize);
        int CountByStatus(BugStatus status);
        Bug GetById(int id);
        IEnumerable<BugGroup> GetBugGroups();
        IEnumerable<ApplicationUser> GetUsers();
        ServiceResult Close(int id);
        DashboardDto GetDashboard(BugFilterDto filter);
    }
}