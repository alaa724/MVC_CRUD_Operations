using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.DAL.Models
{
    // Model
    public class Department : BaseModel
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfeCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
