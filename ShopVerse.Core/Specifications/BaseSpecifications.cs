﻿using ShopVerse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications
{
    public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; } = null;
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnabeld { get ; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>> expression)
        { 
            Criteria = expression;
        }
        public BaseSpecifications()
        {
            
        }
        public void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        public void AddOrderByDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderByDesc = expression;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabeld = true;
            Skip = skip;
            Take = take;
        }
    }
    
}
