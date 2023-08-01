using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Group7WebApp.Helpers;
using Group7WebApp.Helpers.Interface;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Data;

namespace Group7WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<WebAppUser> _userManager;
        private readonly SignInManager<WebAppUser> _signInManager;
        private readonly IAuthorizationMiddlewareService _authorizationService;
        private readonly AuthDbContext _context;
        public UserController(AuthDbContext context, UserManager<WebAppUser> userManager,
            SignInManager<WebAppUser> signInManager, IAuthorizationMiddlewareService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
        }
        [Authorize(Roles = $"{Roles.AdminRole},{Roles.ManagerRole},{Roles.UserRole}")]
        public IActionResult Index()
        {
            if (User.IsInRole(Roles.UserRole))
                return View(_userManager.Users.Where(x => x.ContactId == _userManager.GetUserId(User) || x.Status == Status.Approved.GetDescription()).ToList());
            return View(_userManager.Users.ToList());
        }


        public ActionResult Create()
        {
            var model = new ContactModel()
            {
                RoleList = PopulateRoles()
            };
            return View(model);
        }
        private List<SelectListItem> PopulateRoles(string selected = "")
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem { Text=Roles.UserRole,Value=Roles.UserRole},
                    new SelectListItem { Text=Roles.ManagerRole,Value=Roles.ManagerRole},
                    new SelectListItem { Text=Roles.AdminRole,Value=Roles.AdminRole}

                };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactModel model)
        {
            try
            {
                model.RoleList = PopulateRoles();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                WebAppUser user = new WebAppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirtName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Address = model.Address,
                    State = model.State,
                    City = model.City,
                    Zip = model.Zip,
                    Status = Status.Pending.GetDescription(),
                    ContactId = _userManager.GetUserId(User)
                };

                IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, model.Role).Wait();
                }
                else
                {
                    TempData["error"] = result.Errors?.FirstOrDefault()?.Description;

                    return View(model);
                }

                TempData["success"] = "Contact Created Successfully!!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(AuthorizationModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var user = _userManager.Users.Where(x => x.Email == model.Id).FirstOrDefault();
                user.Status = model.Status;
                _userManager.UpdateAsync(user).Wait();

                TempData["success"] = "Contact Approved Successfully!!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(AuthorizationModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var user = _userManager.Users.Where(x => x.Email == model.Id).FirstOrDefault();
                user.Status = model.Status;
                _userManager.UpdateAsync(user).Wait();

                TempData["success"] = "Contact Rejected Successfully!!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
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



        public ActionResult Edit(string id)
        {
            var user = _userManager.Users.FirstOrDefault(p => p.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditContactModel()
            {
                RoleList = PopulateRoles(),
                Address = user.Address,
                City = user.City,
                Email = user.Email,
                FirstName = user.FirtName,
                LastName = user.LastName,
                Role = user.Role,
                State = user.State,
                Zip = user.Zip,
                Id=user.Id

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditContactModel model)
        {
            try
            {
                model.RoleList = PopulateRoles();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _userManager.Users.FirstOrDefault(p => p.Id == model.Id);

                if (user == null)
                {
                    TempData["error"] = "Contact not found";

                    return View(model);
                }
                string previousRole = user.Role;
                user.Email = model.Email;
                user.FirtName = model.FirstName;
                user.LastName = model.LastName;
                user.Role = model.Role;
                user.State = model.State;
                user.Zip = model.Zip;
                user.Address = model.Address;
                user.City = model.City;
                user.UserName = model.Email;



                IdentityResult result = _userManager.UpdateAsync(user).Result;

                if (result.Succeeded)
                {
                    _userManager.RemoveFromRoleAsync(user, previousRole).Wait();

                    _userManager.AddToRoleAsync(user, model.Role).Wait();
                }
                else
                {
                    TempData["error"] = result.Errors?.FirstOrDefault()?.Description;

                    return View(model);
                }

                TempData["success"] = "Contact Updated Successfully!!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
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
                //  await _signInManager.SignOutAsync();
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
