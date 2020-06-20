using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LabApp.Server.Data;
using Microsoft.EntityFrameworkCore;


namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> AddIncludes<TSource>(this IQueryable<TSource> source,
            params Expression<Func<TSource, object>>[] includes) where TSource : class
        {
            return includes.Aggregate(source, (current, include) => current.Include(include));
        }

        public static IQueryable<TSource> ToPaged<TSource>(this IQueryable<TSource> source, Paging paging)
        {
            if (paging == null) return source;
            return source.Skip(paging.Skip).Take(paging.Take);
        }
        
        public static List<TSource> ToPagedList<TSource>(this IQueryable<TSource> source, Paging paging)
        {
            
            return source.ToPaged(paging).ToList();
        }
        
        public static async Task<List<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> source, Paging paging)
        {
            return await source.ToPaged(paging).ToListAsync();
        }
    }
}