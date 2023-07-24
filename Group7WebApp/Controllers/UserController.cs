using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Group7WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<WebAppUser> _userManager;
        private readonly AuthDbContext _context;
        public UserController(AuthDbContext context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
