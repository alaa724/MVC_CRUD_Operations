using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;
using Route.C41.G01.DAL.Models;
using System;

namespace Route.C41.G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

        // Inheritance : DepartmentRepository is a Controller
        // Composation : DepartmentController has a DepartmentRepository


        public DepartmentController(IDepartmentRepository departmentRepo , IWebHostEnvironment env) // Ask CLR for creating an object from class implementing "IDepartmentRepository" Interface
        {

            _departmentRepo = departmentRepo;
            _env = env;
        }



        // Department/Index

        public IActionResult Index()
        {
            var departments = _departmentRepo.GetAll();
            return View(departments);
        }

        // /Depertment/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var count = _departmentRepo.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // Department/Details/10
        // Department/Details
        //[HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = _departmentRepo.Get(id.Value);
            if (department is null)
                return NotFound();  // 404

            return View(ViewName,department);

        }

        // Department/Edit/10
        // Department/Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id , "Edit");

            ///if (!id.HasValue)
            ///    return BadRequest(); // 400
            ///var department = _departmentRepo.Get(id.Value);
            ///if (department is null)
            ///    return NotFound(); // 404
            ///return View(department);

        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id,Department department)
        {
            if (id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);

            try
            {
                _departmentRepo.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Frindly Message

                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Update The Department ");

                return View(department);

            }
        }
    }
}
