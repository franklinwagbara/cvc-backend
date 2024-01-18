using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.IContracts
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T item);
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
        Task<T> Update(T item);
        Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities);
        Task<bool> Remove(T entity);
        Task<bool> RemoveRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAll(string includeProperties = null);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression, string includeProperties = null);
        IQueryable<T> Query(Expression<Func<T, bool>> expression, string includeProperties = null);
        IQueryable<T> Query();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, string includeProperties = null);
    }
}
