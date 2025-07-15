using ShopVerse.Core.Entities;
using ShopVerse.Core.Respository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IGenericRepository<TEntity,TKey> Repository<TEntity,TKey>() where TEntity : BaseEntity<TKey>;
    }
}
