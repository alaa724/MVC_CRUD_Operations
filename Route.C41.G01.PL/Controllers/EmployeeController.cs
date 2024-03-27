using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeInterface _employeeInterface;
        private readonly IWebHostEnvironment _env;
        //private readonly IDepartmentRepository _departmentRepo;

        public EmployeeController(IMapper mapper , IEmployeeInterface employeeInterface,/*IDepartmentRepository departmentRepo ,*/ IWebHostEnvironment env)// Ask CLR for creating an object from class implementing "IEmployeeInterface"
        {
            _mapper = mapper;
            _employeeInterface = employeeInterface;
            _env = env;
            //_departmentRepo = departmentRepo;
        }

        public IActionResult Index(string searchInp)
        {
            var employee = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInp))
                employee = _employeeInterface.GetAll();
            else
                employee = _employeeInterface.SearchByName(searchInp.ToLower());

            var mappedEmp = _mapper.Map<IEnumerable<Employee> , IEnumerable<EmployeeViewModel>>(employee);
            
             return View(mappedEmp);
        }

        
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var Count = _employeeInterface.Add(mappedEmp);

                // 3. TempData is a Dictionary type property [introduced in ASp.Net Framework 3.5]
                //   => It helps us to to pass data from the controller(Action) to another Controller[Action].

                if (Count > 0)
                    TempData["Message"] = "Employee Created Successfuly";
                else
                    TempData["Message"] = "An Error Occured , Employee Not Created :(";

                return RedirectToAction(nameof(Index));

            }
                return View(employeeVM);


        }

        // Employee/Details/1
        // Employee/Details
        //[HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var employee = _employeeInterface.Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();  // 404

            return View(ViewName, mappedEmp);

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
        public IActionResult Edit([FromRoute] int? id , EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _employeeInterface.Update(mappedEmp);
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

                return View(employeeVM);
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
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _employeeInterface.Delete(mappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured During Deleting This Employee");

                return View(employeeVM);
            }

        }
    }
}
