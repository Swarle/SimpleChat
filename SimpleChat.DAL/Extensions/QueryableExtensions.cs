using Microsoft.EntityFrameworkCore;
using SimpleChat.DAL.Abstract;

namespace SimpleChat.DAL.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> inputQuery,
        BaseSpecification<TEntity> specification) where TEntity : Entity
    {
        var query = inputQuery;
            
        if (specification.Expression != null)
        {
            query = query.Where(specification.Expression);
        }

        if (specification.IncludeString.Count > 0)
        {
            query = specification.IncludeString
                .Aggregate(query,
                    (current, include) => current.Include(include));
        }

        if (specification.OrderBy != null)
        {
            if (specification.IsOrderByDescending)
            {
                if (specification.ThenBy != null)
                    query = specification.IsThenByDescending
                        ? query.OrderByDescending(specification.OrderBy).ThenByDescending(specification.ThenBy)
                        : query.OrderByDescending(specification.OrderBy).ThenBy(specification.ThenBy);
                else
                    query = query.OrderByDescending(specification.OrderBy);

            }
            else
            {
                if (specification.ThenBy != null)
                    query = specification.IsThenByDescending
                        ? query.OrderBy(specification.OrderBy).ThenByDescending(specification.ThenBy)
                        : query.OrderBy(specification.OrderBy).ThenBy(specification.ThenBy);
                else
                    query = query.OrderBy(specification.OrderBy);
            }
        }
        
        return query;
    }
}