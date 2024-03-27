using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Route.C41.G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

        // Inheritance : DepartmentRepository is a Controller
        // Composation : DepartmentController has a DepartmentRepository


        public DepartmentController( IMapper mapper, IDepartmentRepository departmentRepo , IWebHostEnvironment env) // Ask CLR for creating an object from class implementing "IDepartmentRepository" Interface
        {
            _mapper = mapper;
            _departmentRepo = departmentRepo;
            _env = env;
        }



        // Department/Index

        public IActionResult Index()
        {
            var departments = _departmentRepo.GetAll();

            var mappedDept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(mappedDept);
        }

        // /Depertment/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                var count = _departmentRepo.Add(mappedDept);

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }


        // Department/Details/10
        // Department/Details
        //[HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = _departmentRepo.Get(id.Value);

            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
                return NotFound();  // 404

            return View(ViewName,mappedDept);

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
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _departmentRepo.Update(mappedDept);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Frindly Message

                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Updating The Department ");

                return View(departmentVM);

            }
        }


        // Department/Delete/10
        // Department/Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");


        }

        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _departmentRepo.Delete(mappedDept);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exeption
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Deleting The Department ");

                return View(departmentVM);



            }

        }

    }
}
