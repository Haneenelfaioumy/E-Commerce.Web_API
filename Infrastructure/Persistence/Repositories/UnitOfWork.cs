using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            // Get Type Name
            var typeName = typeof(TEntity).Name;
            // Dic<string,Object> => string Key [Name Of Type] -- Object Object From Generic Repository
            if (_repositories.TryGetValue(typeName , out object? value))
                return (IGenericRepository<TEntity, Tkey>)value;
            //if(_repositories.ContainsKey(typeName))
            //    return (IGenericRepository<TEntity, Tkey>) _repositories[typeName];
            else
            {
                // Create Object
                var Repo = new GenericRepository<TEntity , Tkey>(_dbContext);
                // Store Object in Dic
                _repositories["typeName"] = Repo;
                // Return Object
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
