﻿using System;
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

        [Required(ErrorMessage ="Code Is Required !")]
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
