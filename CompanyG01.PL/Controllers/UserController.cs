using AutoMapper;
using CompanyG01.DAL.Models;
using CompanyG01.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyG01.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var Users = await _userManager.Users.Select(
					U => new UserViewModel()
					{
						Id = U.Id,
						FName = U.FName,
						LName = U.LName,
						Email = U.Email,
						PhoneNumber = U.PhoneNumber,
						Roles = _userManager.GetRolesAsync(U).Result
					}).ToListAsync();
				return View(Users);
			}
			else
			{
				var User = await _userManager.FindByEmailAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.FName,
					LName = User.LName,
					Email = User.Email,
					PhoneNumber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result
				};
				return View(new List<UserViewModel> { MappedUser });
			}
		}

		public async Task<IActionResult> Details(string Id, string ViewName = "Details")
		{
			if (Id is null)
				return BadRequest();
			var User = await _userManager.FindByIdAsync(Id);
			if (User is null)
				return NotFound();
			var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
			return View(ViewName, MappedUser);
		}

		public async Task<IActionResult> Edit(string Id)
		{
			return await Details(Id, "Edit");
		}

		[HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string Id)
        {
			if (Id != model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
                    // var MappedUser = _mapper.Map<UserViewModel, ApplicationUser>(model);
                    var User = await _userManager.FindByIdAsync(Id);
					User.PhoneNumber = model.PhoneNumber;
					User.FName = model.FName;
					User.LName = model.LName;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
				catch(Exception e)
				{
					ModelState.AddModelError(string.Empty, e.Message);
				}
			}
			return View(model);
        }

		public async Task<IActionResult> Delete(string Id)
		{
			return await Details(Id, "Delete");
		}

		[HttpPost]
        public async Task<IActionResult> ConfirmDelete(string Id)
        {
			try
			{
				var User = await _userManager.FindByIdAsync(Id);
				await _userManager.DeleteAsync(User);
				return RedirectToAction(nameof(Index));
			}
			catch(Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return RedirectToAction("Error", "Home");
			}
        }
    }
}
