using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Helpers;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        //private readonly IEmployeeInterface _employeeInterface;
        //private readonly IDepartmentRepository _departmentRepo;

        public EmployeeController(
                                IUnitOfWork unitOfWork,
                                IMapper mapper ,
                               //IEmployeeInterface employeeInterface,
                               /*IDepartmentRepository departmentRepo ,*/
                               IWebHostEnvironment env)// Ask CLR for creating an object from class implementing "IEmployeeInterface"
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }

        public IActionResult Index(string searchInp)
        {
            var employee = Enumerable.Empty<Employee>();

            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;

            if (string.IsNullOrEmpty(searchInp))
                employee = _unitOfWork.Repository<Employee>().GetAll();
            else
                employee = employeeRepo.SearchByName(searchInp.ToLower());

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
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image , "Images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);


               _unitOfWork.Repository<Employee>().Add(mappedEmp);

                // 2. Update Department
                // _unitOfWork.Repository<Department>.Update(mappedDept);

                // 3. Delete Project
                // _unitOfWork.Repository<Project>.Remove(mappedEmp);

                // _dbcontext.SaveChanges();
                var Count = _unitOfWork.Complete();

                if(Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

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

            var employee = _unitOfWork.Repository<Employee>().Get(id.Value);

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

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                _unitOfWork.Complete();
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

                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                _unitOfWork.Complete();
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
