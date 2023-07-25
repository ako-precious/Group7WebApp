using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Polly;
using Group7WebApp.Models;
using System.Data;

namespace Group7WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersRole : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<WebAppUser> _userManager;


        public UsersRole( RoleManager<IdentityRole> roleManager, UserManager<WebAppUser> userManager)
        {            
            _roleManager = roleManager;
            _userManager = userManager;
           
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        public IActionResult Details(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(p => p.Id == id);


            if (role == null)
            {
                return NotFound();
            }
            var usersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result;

            ViewBag.RoleName = role.Name;
            ViewBag.UsersInRole = usersInRole;

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {

            if (ModelState.IsValid & !_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
                
                TempData["success"] = "Role created successfully";
            }
                return RedirectToAction(nameof(Index));
            
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(p => p.Id == id);

            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model )
        {
            var existingRole = await _roleManager.FindByIdAsync(model.Id);

            if (existingRole == null)
            {
                // Handle role not found error
                return NotFound();
            }
            // Update the role properties
            existingRole.Name = model.Name;

            // Save the changes
            var result = await _roleManager.UpdateAsync(existingRole);

            if (result.Succeeded)
            {
                // Role updated successfully
                TempData["success"] = "Role Edited successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(p => p.Id == id);

            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(IdentityRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    // Role deleted successfully
                    TempData["success"] = "Role Deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                // Role not found
                return NotFound();
            }

            }

    }

}
