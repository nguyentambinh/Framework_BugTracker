using BugTracker.Core.Base;
using BugTracker.Core.DTOs;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.Interfaces;
using BugTracker.Core.ViewModels;
using BugTracker.Data.Context;
using BugTracker.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;

namespace BugTracker.Service.Services
{
    public class BugService : IBugService
    {
        private readonly BugTrackerDbContext _context;
        private readonly IPermissionService _permissionService;

        public BugService(
    IUserContext userContext,
    IPermissionService permissionService)
        {
            _context = new BugTrackerDbContext(userContext);
            _permissionService = permissionService;
        }

        public ServiceResult Create(CreateBugDto dto, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ServiceResult.Fail("Tiêu đề bug không được để trống");

            var bug = new Bug
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,

                Status = BugStatus.Open,

                BugGroupId = dto.BugGroupId,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = currentUserId,
                CreatedDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Bugs.Add(bug);
            _context.SaveChanges();

            return ServiceResult.Ok("Tạo mới thành công");
        }
        public ServiceResult Edit(EditBugDto dto, int currentUserId)
        {
            if (dto == null)
                return ServiceResult.Fail("Dữ liệu không hợp lệ");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return ServiceResult.Fail("Tiêu đề bug không được để trống");

            var bug = _context.Bugs.FirstOrDefault(x => x.Id == dto.Id);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            if (bug.Status == BugStatus.Closed)
                return ServiceResult.Fail("Bug đã đóng, không thể chỉnh sửa");

            bug.Title = dto.Title.Trim();
            bug.Description = dto.Description;
            bug.Priority = (BugPriority)dto.Priority;

            _context.SaveChanges();

            return ServiceResult.Ok("Sửa thành công");
        }
        public ServiceResult Assign(int bugId, int assignToUserId)
        {
            var bug = _context.Bugs.Find(bugId);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            bug.AssignedToUserId = assignToUserId;
            bug.Status = BugStatus.InProgress;

            _context.SaveChanges();
            return ServiceResult.Ok("Phân công thành công");
        }

        public ServiceResult ChangeStatus(int bugId, BugStatus newStatus)
        {
            var bug = _context.Bugs.Find(bugId);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            var permissionResult = _permissionService.CanChangeStatus(bug, newStatus);

            if (!permissionResult.Success)
                return ServiceResult.Fail(permissionResult.Message);

            bug.Status = newStatus;
            _context.SaveChanges();

            return ServiceResult.Ok("Đổi trạng thái bug thành công");
        }
        public List<UserPermissionViewModel> GetUserPermissions()
        {
            return (from u in _context.Users
                    join rp in _context.RolePermissions on u.RoleId equals rp.RoleId into JoinedRP
                    from p in JoinedRP.DefaultIfEmpty()
                    where !u.IsDeleted
                    select new UserPermissionViewModel
                    {
                        Id = u.Id,
                        DisplayName = u.DisplayName,
                        CanOpen = p != null ? p.CanOpen : false,
                        CanInProgress = p != null ? p.CanInProgress : false,
                        CanFixed = p != null ? p.CanFixed : false,
                        CanClosed = p != null ? p.CanClosed : false
                    }).ToList();
        }
        public bool CanChangeStatusFlag(BugStatus status)
{
    return _permissionService.CanChangeStatusFlag(status);
}
        public IEnumerable<Bug> GetAll()
        {
            return _context.Bugs
                .Include(x => x.BugGroup)
                .Include(x => x.CreatedByUser)
                .Include(x => x.AssignedToUser)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

        public IEnumerable<Bug> GetOpenBugs()
        {
            return _context.Bugs
                .Where(x => x.Status == BugStatus.Open)
                .ToList();
        }
        public PagedResult<Bug> GetPaged(int page, int pageSize)
        {
            var query = _context.Bugs
                .Include(x => x.CreatedByUser)
                .Include(x => x.AssignedToUser)
                .OrderByDescending(x => x.CreatedDate);

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Bug>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public int CountByStatus(BugStatus status)
        {
            return _context.Bugs.Count(x => x.Status == status);
        }
        public Bug GetById(int id)
        {
            return _context.Bugs
                .Include(x => x.BugGroup)
                .Include(x => x.CreatedByUser)
                .Include(x => x.AssignedToUser)
                .FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<BugGroup> GetBugGroups()
        {
            return _context.BugGroups
                .OrderBy(x => x.Name)
                .ToList();
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.Users
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayName)
                .ToList();
        }
        public ServiceResult Close(int id)
        {
            var bug = _context.Bugs.Find(id);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            bug.Status = BugStatus.Closed;

            _context.SaveChanges(); 

            return ServiceResult.Ok();
        }
        public DashboardDto GetDashboard(BugFilterDto filter)
        {
            var query = _context.Bugs.AsQueryable();

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status);

            if (filter.GroupId.HasValue)
                query = query.Where(x => x.BugGroupId == filter.GroupId);

            if (filter.UserId.HasValue)
                query = query.Where(x => x.AssignedToUserId == filter.UserId);

            var bugs = query.ToList();

            return new DashboardDto
            {
                Bugs = bugs,
                BugGroups = _context.BugGroups.ToList(),
                Users = _context.Users.ToList(),
                TotalBugs = bugs.Count,
                OpenBugs = bugs.Count(x => x.Status == BugStatus.Open),
                InProgressBugs = bugs.Count(x => x.Status == BugStatus.InProgress),
                ClosedBugs = bugs.Count(x => x.Status == BugStatus.Closed)
            };
        }
    }
}