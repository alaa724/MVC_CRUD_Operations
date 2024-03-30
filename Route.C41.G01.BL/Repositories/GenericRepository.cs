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
            

        public T Get(int id)
        {
            return _dbContext.Find<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        
    }
}
