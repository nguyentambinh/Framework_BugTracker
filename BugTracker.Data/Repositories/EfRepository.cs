using System.Data.Entity;
using System.Linq;
using BugTracker.Core.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.Core.Base;
using BugTracker.Data.Context;

namespace BugTracker.Data.Repositories
{
    public class EfRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly BugTrackerDbContext _context;

        public EfRepository(BugTrackerDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted);
        }

        public T GetById(int id)
        {
            return _context.Set<T>()
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = true; // Soft delete
                Update(entity);
            }
        }

        public IQueryable<T> ApplySpecification(BaseSpecification<T> spec)
        {
            IQueryable<T> query = _context.Set<T>();

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);

            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);

            return query;
        }
    }
}