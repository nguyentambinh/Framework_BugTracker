using BugTracker.Core.Base;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.DTOs;
using System.Collections.Generic;

namespace BugTracker.Core.Interfaces
{
    public interface IBugService
    {
        ServiceResult Create(CreateBugDto dto, int currentUserId);
        ServiceResult Assign(int bugId, int assignToUserId);
        ServiceResult ChangeStatus(int bugId, BugStatus newStatus);

        IEnumerable<Bug> GetAll();
        IEnumerable<Bug> GetOpenBugs();
        PagedResult<Bug> GetPaged(int page, int pageSize);
        int CountByStatus(BugStatus status);
        Bug GetById(int id);
        IEnumerable<BugGroup> GetBugGroups();
        IEnumerable<ApplicationUser> GetUsers();
    }
}