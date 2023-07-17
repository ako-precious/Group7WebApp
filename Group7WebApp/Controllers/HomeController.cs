using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Group7WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<WebAppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<WebAppUser> UserManager)
        {
            _logger = logger;
            _userManager = UserManager;
        }

        public IActionResult Index()
        {
            var user_id= _userManager.GetUserId(HttpContext.User);
            WebAppUser user = _userManager.FindByIdAsync(user_id).Result;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}