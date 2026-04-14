using Ecommerce.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        Task SaveChangesAsync();
    }
}
