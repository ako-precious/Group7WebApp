using Group7WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Group7WebApp.Data;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Group7WebApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public PostController(AuthDbContext context, UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Posts
        public IActionResult Index()
        {
            var posts = _context.Posts.Include(p => p.Categories).ToList();
            return View(posts);
        }

        // GET: /Posts/Details/5
        public IActionResult Details(Guid id)
        {
            var post = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
            ViewBag.Categories = _context.Categories.ToList();
            
            if (post == null)
            {
                return NotFound();
            }
           
           

            return View();
        }

        [Authorize(Roles = "Admin,Editor")]
        // GET: /Posts/Create
        public IActionResult Create()
        {
            
            var userId = _userManager.GetUserId(HttpContext.User);
           

            // Fetch the user details from the database using the userId
            ViewBag.user = _userManager.FindByIdAsync(userId).Result;
            ViewBag.Categories = _context.Categories.ToList();

            if (userId == null)
            {
                return NotFound(); // User not found, handle the error accordingly
            }
           
            return View();
        }

        [Authorize(Roles = "Admin,Editor")]
        // POST: /Posts/Create
        [HttpPost]
        public async Task<object> Create(Post post, Guid[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                // Add selected categories to the post
                post.Categories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();


                _context.Posts.Add(post);
                _context.SaveChanges();
                        
                var postId = _context.Posts.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                
                
                TempData["success"] = "Category created successfully";


                return RedirectToAction("Index");
            }
            
            return View(post);
        }

        [Authorize(Roles = "Admin,Editor")]
        // GET: /Posts/Edit/5
        public IActionResult Edit(Guid id)
        {
            var post = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
            ViewBag.Categories = _context.Categories.ToList();
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }
        [Authorize(Roles = "Admin,Editor")]

        // POST: /Posts/Edit/5
        [HttpPost]
        public IActionResult Edit(Post updatedPost, Guid[] categoryIds)
        {
            
            if (ModelState.IsValid)
            {
                //var post = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);

                //if (post == null)
                //{
                //    return NotFound();
                //}

                // Update the post's categories
                //post.Categories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();

                _context.Posts.Update(updatedPost);
                _context.SaveChanges();
                TempData["success"] = "Post Edited successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(updatedPost);
        }
        [Authorize(Roles = "Admin,Editor")]
        // GET: /Posts/Delete/5
        public IActionResult Delete(Guid id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: /Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();
            TempData["success"] = "Post Deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}

