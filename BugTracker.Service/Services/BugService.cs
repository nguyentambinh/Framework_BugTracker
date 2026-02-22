using BugTracker.Core.Base;
using BugTracker.Core.DTOs;
using BugTracker.Core.Entities;
using BugTracker.Core.Enums;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using BugTracker.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;

namespace BugTracker.Service.Services
{
    public class BugService : IBugService
    {
        private readonly BugTrackerDbContext _context;

        public BugService(IUserContext userContext)
        {
            _context = new BugTrackerDbContext(userContext);
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

            return ServiceResult.Ok();
        }

        public ServiceResult Assign(int bugId, int assignToUserId)
        {
            var bug = _context.Bugs.Find(bugId);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            bug.AssignedToUserId = assignToUserId;
            bug.Status = BugStatus.InProgress;

            _context.SaveChanges();
            return ServiceResult.Ok();
        }

        public ServiceResult ChangeStatus(int bugId, BugStatus newStatus)
        {
            var bug = _context.Bugs.Find(bugId);
            if (bug == null)
                return ServiceResult.Fail("Bug không tồn tại");

            bug.Status = newStatus;
            _context.SaveChanges();

            return ServiceResult.Ok();
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