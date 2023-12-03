using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bunkering.Access.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        internal DbSet<T> _db;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
        {
            _db.AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
            return entities;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression, string includeProperties = null)
        {
            IQueryable<T> query = _db;

            if (expression != null)
                query = query.Where(expression);

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, string includeProperties = null)
        {
            IQueryable<T> query = _db;

            if (expression != null)
                query = query.Where(expression);

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(string includeProperties = null)
        {
            IQueryable<T> query = _db.Select(x => x);

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            return query;
        }

        public async Task<bool> Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _db.Attach(entity);

            _db.Remove(entity);
            return true;
        }

        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            _db.AttachRange(entities);
            _db.RemoveRange(entities);
            return true;
        }

    }
}
