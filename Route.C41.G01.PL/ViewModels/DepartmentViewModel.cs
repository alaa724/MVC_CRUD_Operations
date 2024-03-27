using Route.C41.G01.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Route.C41.G01.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required !")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required !")]
        public string Name { get; set; }

        [Display(Name = "Date Of Creation")]
        public DateTime DateOfeCreation { get; set; }

        [InverseProperty(nameof(Employee.Department))]
        // Navigational Property [Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
