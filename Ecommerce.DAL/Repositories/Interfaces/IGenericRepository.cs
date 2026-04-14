using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ecommerce.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<TEntity?> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? condition = null);


        // this overload for include related entities using eager loading , expect to create a new method for each entity to include related entities using eager loading but this method is generic and can be used for all entities
        Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>>? condition,
            params Expression<Func<TEntity, object>>[] includes);
        void Add(TEntity entity);
        void Update(TEntity entity);    
        void Delete(TEntity entity);
    }
}
