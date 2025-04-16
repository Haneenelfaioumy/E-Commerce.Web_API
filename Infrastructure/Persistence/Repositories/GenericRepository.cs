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

        public async Task<TEntity?> GetById(Tkey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
      
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
    }
}
