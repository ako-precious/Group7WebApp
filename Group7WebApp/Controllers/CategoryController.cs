using Group7WebApp.Data;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group7WebApp.Controllers
{
     [Authorize]
    public class CategoryController : Controller
    {
        private readonly AuthDbContext _dbContext;

        public CategoryController(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> category = _dbContext.Categories;
            return View(category);
        }

        public IActionResult Details(Guid id)
        {
            var category = _dbContext.Categories.Include(p => p.Posts).FirstOrDefault(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "Admin,Editor")]
        //GET
        public IActionResult Create()
        {

            return View();
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(obj);
                _dbContext.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _dbContext.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(obj);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["success"] = "Category edit successfully";
            return View(obj);
        }
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Delete(Guid? id)
        {

            var obj = _dbContext.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
