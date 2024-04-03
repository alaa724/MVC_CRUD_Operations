using Microsoft.EntityFrameworkCore;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BL.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseModel 
    {
        private protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public void Add(T entity)

            => _dbContext.Set<T>().Add(entity);
           //return _dbContext.SaveChanges();
        

        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);
         //return _dbContext.SaveChanges();
        

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);
            //return _dbContext.SaveChanges();
            

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.FindAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            else
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        
    }
}
