using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
       
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SaveChangesAsync();
    }
}
