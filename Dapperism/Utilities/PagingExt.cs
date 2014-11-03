using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dapperism.Utilities
{
    public static class PagingExt
    {
        public static IEnumerable<T> PagedList<T, TResult>(this IQueryable<T> source,
            int pageIndex, int pageSize, Expression<Func<T, TResult>> orderByProperty
            , bool isAscendingOrder, out int rowsCount)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException("pageIndex must be greater than zero");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize must be greater than zero");
            }

            var src = source;

            rowsCount = source.Count();

            src = isAscendingOrder ? src.OrderBy(orderByProperty) : src.OrderByDescending(orderByProperty);

            var result = src.Skip((pageIndex - 1)*pageSize).Take(pageSize);

            return result;
        }
    }
}
