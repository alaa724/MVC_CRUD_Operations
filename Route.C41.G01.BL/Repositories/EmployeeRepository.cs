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
    public class EmployeeRepository : IEmployeeInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;

        }

        public int Add(Employee entity)
        {
            _dbContext.Employees.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(Employee entity)
        {
            _dbContext.Employees.Update(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee entity)
        {
            _dbContext.Employees.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public Employee Get(int id)
        {
            return _dbContext.Find<Employee>(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _dbContext.Employees.AsNoTracking().ToList();
        }

    }
}
