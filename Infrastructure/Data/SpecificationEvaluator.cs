using System;
using Core.Entities;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // x.Brand == "brand"
        }
        // TODO: Add includes later
        // query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
