using AutoMapper;
using CompanyG01.BLL.Interfaces;
using CompanyG01.BLL.Repositories;
using CompanyG01.DAL.Models;
using CompanyG01.PL.Helpers;
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
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.SearchEmployeesByName(SearchValue);
            }
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid)
            {
                // Mapping from EmployeeViewModel to Employee

                // Manual Mapping
                //var employee = new Employee()
                //{
                //    Name = EmployeeVM.Name,
                //    Age = EmployeeVM.Age,
                //    Address = EmployeeVM.Address,
                //    Email = EmployeeVM.Email,
                //    PhoneNumber = EmployeeVM.PhoneNumber,
                //    DepartmentId = EmployeeVM.DepartmentId,
                //    IsActive = EmployeeVM.IsActive,
                //    HireDate = EmployeeVM.HireDate
                //};

                // Upload image => wwwroot
                EmployeeVM.ImageName = DocumentSettings.UploadFile(EmployeeVM.Image, "images");
                
                // Auto Mapper
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                await _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Added";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(EmployeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound();

            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName, MappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    await _unitOfWork.Complete();
                    TempData["Message"] = "Employee Is Updated";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(EmployeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel EmployeeVM, [FromRoute] int id)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();

            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                if (MappedEmployee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(MappedEmployee.ImageName, "images");
                }
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                await _unitOfWork.Complete();
                TempData["Message"] = "Employee Is Deleted";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(EmployeeVM);
            }
        }
    }
}
