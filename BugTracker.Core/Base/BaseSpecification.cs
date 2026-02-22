using System;
using System.Linq.Expressions;

namespace BugTracker.Core.Base
{
    public abstract class BaseSpecification<T>
    {
        // Điều kiện lọc chính (WHERE)
        public Expression<Func<T, bool>> Criteria { get; protected set; }

        // Sắp xếp tăng dần
        public Expression<Func<T, object>> OrderBy { get; protected set; }

        // Sắp xếp giảm dần
        public Expression<Func<T, object>> OrderByDescending { get; protected set; }

        // Phân trang
        public int? Take { get; protected set; }
        public int? Skip { get; protected set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDescending = orderByDesc;
        }
    }
}