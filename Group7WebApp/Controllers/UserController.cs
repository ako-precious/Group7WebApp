using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Data;

namespace Group7WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<WebAppUser> _userManager;
        private readonly SignInManager<WebAppUser> _signInManager;

        private readonly AuthDbContext _context;
        public UserController(AuthDbContext context, UserManager<WebAppUser> userManager, SignInManager<WebAppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {

            return View(_userManager.Users.ToList());
        }

        public IActionResult Details(string id)
        {
            var user = _userManager.Users.FirstOrDefault(p => p.Id == id);
            
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public IActionResult Delete(string id)
        {
            var user = _userManager.Users.FirstOrDefault(p => p.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WebAppUser userr)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userr.Id);

            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // If the user was successfully deleted, you can perform additional actions if needed.
                // For example, you can sign out the user (optional).
                await _signInManager.SignOutAsync();
                TempData["success"] = "User Account Deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // If there are errors during the deletion process, handle them appropriately.
                // For example, you can display an error message to the user.
                ModelState.AddModelError("", "User deletion failed. Please try again.");
                return View();
            }
        }

    }
}
