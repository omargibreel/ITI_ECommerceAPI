using Ecommerce.DAL.Repositories.Classes;
using Ecommerce.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);

            if(_repositories.TryGetValue(entityType, out var repository))
                return (IGenericRepository<TEntity>) repository;

            var newRepo = new GenericRepository<TEntity>(_context);

            _repositories[entityType] = newRepo;

            return newRepo;


        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
