using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.DAL.Models;
using System;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeInterface _employeeInterface;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeInterface employeeInterface , IWebHostEnvironment env)// Ask CLR for creating an object from class implementing "IEmployeeInterface"
        {
            _employeeInterface = employeeInterface;
            _env = env;
        }

        public IActionResult Index()
        {
            // Binding Throw View's Dictionary : Transfare Date From Action To View [One Way]

            // 1. ViewData is a Dictionary type property [introduced in ASp.Net Framework 3.5]
            //   => It helps us to to pass data from the controller(Action) to the corresponding view.

            ViewData["Message"] = "Hello ViewData";

            // 2. ViewBag is a Dynamic type property [introduced in ASp.Net Framework 4.0]
            //   => It helps us to to pass data from the controller(Action) to the corresponding view.
            ViewBag.Message = "Hello ViewBag";

            var employee = _employeeInterface.GetAll();
            return View(employee);
        }

        
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var Count = _employeeInterface.Add(employee);
                if (Count > 0)
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
