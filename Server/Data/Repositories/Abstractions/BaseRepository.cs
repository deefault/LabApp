/*using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Server.Data.Specifications.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Data.Repositories.Abstractions
{
    public class EfRepository<T> : IRepo<T> where T : class
    {
        protected readonly AppDbContext _db;

        public EfRepository(AppDbContext db)
        {
            _db = db;
        }

        public virtual T GetById(int groupId)
        {
            return _db.Set<T>().Find(groupId);
        }
        
        public virtual async Task<T> GetByIdAsync(int groupId)
        {
            return await _db.Set<T>().FindAsync(groupId);
        }

        public async Task<IEnumerable<T>> List()
        {
            return await _db.Set<T>().ToListAsync();
        }
        
        public IEnumerable<T> List(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IEnumerable<T>> List(ISpecification<T> spec)
        {
            return await _db.Set<T>().ToListAsync();
        }
        
        public  IEnumerable<T> List()
        {
            return _db.Set<T>().ToList();
        }
        
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_db.Set<T>().AsQueryable(), spec);
        }
    }
}*/