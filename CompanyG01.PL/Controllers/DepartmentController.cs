using AutoMapper;
using CompanyG01.BLL.Interfaces;
using CompanyG01.BLL.Repositories;
using CompanyG01.DAL.Models;
using CompanyG01.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyG01.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDepartments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel DepartmentVM)
        {
            if(ModelState.IsValid) // Server side validation
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                await _unitOfWork.DepartmentRepository.Add(MappedDepartment);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    // 3. TempData => Dictionary Object
                    // Transfer data from actio to action
                    TempData["Message"] = "Department Is Created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(DepartmentVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound();

            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, MappedDepartment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int? id, DepartmentViewModel DepartmentVM)
        {
            if (id != DepartmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                    await _unitOfWork.Complete();
                    TempData["Message"] = "Department Is Updated";
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(DepartmentVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel DepartmentVM, [FromRoute]int id)
        {
            if (id != DepartmentVM.Id)
                return BadRequest();

            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                await _unitOfWork.Complete();
                TempData["Message"] = "Department Is Deleted";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(DepartmentVM);
            }
        }
    }
}
