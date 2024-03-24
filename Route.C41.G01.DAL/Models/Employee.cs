using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.DAL.Models
{
    public enum Gender
    {
        [EnumMember(Value ="Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }

    public enum ContractType
    {
        FullTime = 1,
        PartTime = 2
    }
    public class Employee : BaseModel
    {

        [Required(ErrorMessage ="Name is required !!")]
        [MaxLength(50 , ErrorMessage ="Max length of name is 50 char")]
        [MinLength(4 , ErrorMessage = "Min length of name is 4 char")]
        public string Name { get; set; }

        [Range(22 , 60)]
        public int? Age { get; set; }

        public Gender Gender { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s\-\(\)\\/.,]+$"
            , ErrorMessage = "Address Must be like 123 Main St.")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name ="Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name ="Hiring Date")]
        public DateTime HiringDate { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}
