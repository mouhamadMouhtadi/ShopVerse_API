using Microsoft.EntityFrameworkCore;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Respository.Interfaces;
using ShopVerse.Core.Specifications;
using ShopVerse.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Repository.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity:BaseEntity<TKey>
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>) await _context.Products.OrderBy(p=>p.Name).Include(p=>p.Brand).Include(p=>p.Type).ToListAsync();  
            }
            return  await _context.Set<TEntity>().ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> spec)
        {
            return  await ApplySpecification(spec).ToListAsync();
        }
        public async Task<TEntity> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync(p=>p.Id == id as int?) as TEntity ;
            }
            return await _context.Set<TEntity>().FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Update(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Remove(entity);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
           return await ApplySpecification(spec).CountAsync();
        }
    }
}
