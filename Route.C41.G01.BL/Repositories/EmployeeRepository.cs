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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeInterface
    {
        //private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext) // Ask CLR For Creating Object From DbContext
                      : base(dbContext)
        {
            //_dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
            => await _dbContext.Set<Employee>().Include(E => E.Department).AsNoTracking().ToListAsync();

        public IQueryable<Employee> SearchByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
    }
}
