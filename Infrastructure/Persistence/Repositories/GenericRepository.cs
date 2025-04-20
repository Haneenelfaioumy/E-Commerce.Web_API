using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext _dbContext) 
                                  : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Tkey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
      
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        #region With Specifications
         
        async Task<IEnumerable<TEntity>> IGenericRepository<TEntity, Tkey>.GetAllAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();
        }

        async Task<TEntity?> IGenericRepository<TEntity, Tkey>.GetByIdAsync(ISpecifications<TEntity, Tkey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        #endregion

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> specifications)
         => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>() , specifications).CountAsync();

    }
}
