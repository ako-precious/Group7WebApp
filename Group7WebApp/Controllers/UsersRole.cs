using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Group7WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersRole : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRole( RoleManager<IdentityRole> roleManager)
        {            
            _roleManager = roleManager;
           
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {

            if (ModelState.IsValid || !_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }
    }
}
