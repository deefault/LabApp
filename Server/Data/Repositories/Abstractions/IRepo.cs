using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LabApp.Server.Data.Specifications.Abstractions;

namespace LabApp.Server.Data.Repositories.Abstractions
{
    public interface IRepo<T>
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> List();
        Task<IQueryable<T>> ListAsync();
        IEnumerable<T> List(ISpecification<T> spec);
        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        int Count(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}