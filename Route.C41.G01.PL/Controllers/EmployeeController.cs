﻿using AutoMapper;
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
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(string searchInp)
        {
            var employee = Enumerable.Empty<Employee>();

            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;

            if (string.IsNullOrEmpty(searchInp))
                employee = await employeeRepo.GetAllAsync();
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image , "Images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);


               _unitOfWork.Repository<Employee>().Add(mappedEmp);

                // 2. Update Department
                // _unitOfWork.Repository<Department>.Update(mappedDept);

                // 3. Delete Project
                // _unitOfWork.Repository<Project>.Remove(mappedEmp);

                // _dbcontext.SaveChanges();
                var Count = await _unitOfWork.Complete();

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
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var employee = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();  // 404

            if(ViewName.Equals("Delete" , StringComparison.OrdinalIgnoreCase))
                TempData["ImageName"] = employee.ImageName;

            return View(ViewName, mappedEmp);

        }


        // Employee/Edit/1
        // Employee/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepo.GetAll();
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id , EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                await _unitOfWork.Complete();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                var Count = await _unitOfWork.Complete();

                if(Count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);

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
