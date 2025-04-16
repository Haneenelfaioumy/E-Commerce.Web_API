using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity:BaseEntity<Tkey>;
        Task<int> SaveChangesAsync();
    }
}
