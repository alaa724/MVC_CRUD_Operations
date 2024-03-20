using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;

namespace Route.C41.G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;

        // Inheritance : DepartmentRepository is a Controller
        // Composation : DepartmentController has a DepartmentRepository


        public DepartmentController(IDepartmentRepository departmentRepo) // Ask CLR for creating an object from class implementing "IDepartmentRepository" Interface
        {

            _departmentRepo = departmentRepo;
        }



        // Department/Index

        public IActionResult Index()
        {
            return View();
        }
    }
}
