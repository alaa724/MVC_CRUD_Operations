using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.DAL.Models;
using System;
using System.Linq;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeInterface _employeeInterface;
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeInterface employeeInterface,/*IDepartmentRepository departmentRepo ,*/ IWebHostEnvironment env)// Ask CLR for creating an object from class implementing "IEmployeeInterface"
        {
            _employeeInterface = employeeInterface;
            //_departmentRepo = departmentRepo;
            _env = env;
        }

        public IActionResult Index(string searchInp)
        {
            var employee = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInp))
                employee = _employeeInterface.GetAll();
            else
                employee = _employeeInterface.SearchByName(searchInp);
            
             return View(employee);
        }

        
        //[HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepo.GetAll();
            //ViewBag.Departments = _departmentRepo.GetAll();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var Count = _employeeInterface.Add(employee);

                // 3. TempData is a Dictionary type property [introduced in ASp.Net Framework 3.5]
                //   => It helps us to to pass data from the controller(Action) to another Controller[Action].

                if (Count > 0)
                    TempData["Message"] = "Employee Created Successfuly";
                else
                    TempData["Message"] = "An Error Occured , Employee Not Created :(";

                return RedirectToAction(nameof(Index));

            }
                return View();


        }

        // Employee/Details/1
        // Employee/Details
        //[HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var employee = _employeeInterface.Get(id.Value);
            if (employee is null)
                return NotFound();  // 404

            return View(ViewName, employee);

        }


        // Employee/Edit/1
        // Employee/Edit
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepo.GetAll();
            return Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id , Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                _employeeInterface.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Frindly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "OOPS :( An Error Occured During Updating Employee");

                return View(employee);
            }
        }


        // Employee/Delete/10
        // Employee/Delete
        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _employeeInterface.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured During Deleting This Employee");

                return View(employee);
            }

        }
    }
}
