using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        //private readonly IDepartmentRepository _departmentRepo;

        // Inheritance : DepartmentRepository is a Controller
        // Composation : DepartmentController has a DepartmentRepository


        public DepartmentController( 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            //IDepartmentRepository departmentRepo ,
            IWebHostEnvironment env) // Ask CLR for creating an object from class implementing "IDepartmentRepository" Interface
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            //_departmentRepo = departmentRepo;
        }



        // Department/Index

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();

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
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.Repository<Department>().Add(mappedDept);

                var count = await _unitOfWork.Complete();

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }


        // Department/Details/10
        // Department/Details
        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);

            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
                return NotFound();  // 404

            return View(ViewName,mappedDept);

        }

        // Department/Edit/10
        // Department/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id , "Edit");

            ///if (!id.HasValue)
            ///    return BadRequest(); // 400
            ///var department = _departmentRepo.Get(id.Value);
            ///if (department is null)
            ///    return NotFound(); // 404
            ///return View(department);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.Repository<Department>().Update(mappedDept);

                await _unitOfWork.Complete();

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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");


        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.Repository<Department>().Delete(mappedDept);

                await _unitOfWork.Complete();

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
