using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group7WebApp.Controllers
{
    public class PostCategoryController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public PostCategoryController(AuthDbContext context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var postCategories = _context.PostCategories.Include(p => p.Post).Include(p => p.Category).ToList();
                     
            return View(postCategories);
        }
    }
}
