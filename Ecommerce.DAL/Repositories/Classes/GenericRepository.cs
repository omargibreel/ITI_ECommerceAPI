using Ecommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ecommerce.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) => _context = context;

        public void Add(TEntity entity)
            => _context.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);  

        public async Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? condition = null)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            return condition == null
                ? await query.ToListAsync()
                : await query.Where(condition).ToListAsync();
        }


        // this include eager loading method is used when we want to load related entities along with the main entity.
        // i think this is better than create separate methods for each entity with its related entities, because it is more flexible and reusable.
        public async Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? condition,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            foreach (var include in includes)
                query = query.Include(include);
            return condition == null
                ? await query.ToListAsync()
                : await query.Where(condition).ToListAsync();
        }

        public async Task<TEntity?> GetById(int id)
            => await _context.Set<TEntity>().FindAsync(id);

    }
}
