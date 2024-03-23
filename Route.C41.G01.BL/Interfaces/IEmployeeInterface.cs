using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BL.Interfaces
{
    public interface IEmployeeInterface
    {
        IEnumerable<Employee> GetAll();

        Employee Get(int id);

        int Add(Employee entity);

        int Update(Employee entity);

        int Delete(Employee entity);
    }
}
