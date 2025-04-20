using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Create Query
        public static IQueryable<TEntity> CreateQuery<TEntity , TKey>
            (IQueryable<TEntity> InputQuery , ISpecifications<TEntity , TKey> specifications)
            where TEntity : BaseEntity<TKey>
        {
            var Query = InputQuery;
            if(specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }
            if (specifications.OrderBy is not null)
            {
                Query = Query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specifications.OrderByDescending);
            }
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                //foreach(var exp in specifications.IncludeExpressions)
                //    Query = Query.Include(exp);

                Query = specifications
                          .IncludeExpressions
                          .Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp);
            }
            return Query;
        }
    }
}
