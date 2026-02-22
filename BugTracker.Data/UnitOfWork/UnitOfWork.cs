using System;
using BugTracker.Core.Base;
using BugTracker.Core.Entities;
using BugTracker.Data.Context;
using BugTracker.Data.Repositories;

namespace BugTracker.Data.UnitOfWork
{
    /// <summary>
    /// Quản lý DbContext + Repository
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private readonly BugTrackerDbContext _context;

        private EfRepository<Bug> _bugRepository;
        private EfRepository<BugComment> _bugCommentRepository;
        private EfRepository<ApplicationUser> _userRepository;

        public UnitOfWork(BugTrackerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Repository cho Bug
        /// </summary>
        public EfRepository<Bug> Bugs
        {
            get
            {
                if (_bugRepository == null)
                    _bugRepository = new EfRepository<Bug>(_context);

                return _bugRepository;
            }
        }

        /// <summary>
        /// Repository cho Comment của Bug
        /// </summary>
        public EfRepository<BugComment> BugComments
        {
            get
            {
                if (_bugCommentRepository == null)
                    _bugCommentRepository = new EfRepository<BugComment>(_context);

                return _bugCommentRepository;
            }
        }

        /// <summary>
        /// Repository cho User nghiệp vụ
        /// </summary>
        public EfRepository<ApplicationUser> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new EfRepository<ApplicationUser>(_context);

                return _userRepository;
            }
        }

        /// <summary>
        /// Lưu toàn bộ thay đổi
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}