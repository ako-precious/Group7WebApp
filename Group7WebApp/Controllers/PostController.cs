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

namespace Group7WebApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;
        private Task<WebAppUser?> user;

        public PostController(AuthDbContext context, UserManager<WebAppUser> userManager, Task<WebAppUser?> user)
        {
            _context = context;
            _userManager = userManager;
            this.user = user;
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
            var userId = _userManager.GetUserId(User);

            // Fetch the user details from the database using the userId
            user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (post == null)
            {
                return NotFound();
            }
            var viewModel = new PostInformation
            {
                user = user,
                Category = ViewBag.Categories,
                Post = post

            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Editor")]
        // GET: /Posts/Create
        public IActionResult Create()
        {
            
            var userId = _userManager.GetUserId(User);
            

            // Fetch the user details from the database using the userId
            ViewBag.user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
           

            if (User == null)
            {
                return NotFound(); // User not found, handle the error accordingly
            }
           
            return View();
        }

        // POST: /Posts/Create
        [HttpPost]
        public IActionResult Create(Post post, Guid[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                // Add selected categories to the post
                //post.Categories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();


                _context.Posts.Add(post);
                _context.SaveChanges();
                TempData["success"] = "Category created successfully";

                //var latestId = _context.Posts.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                return RedirectToAction("Index");
            }
            
            return View(post);
        }

    // GET: /Posts/Edit/5
    public IActionResult Edit(Guid id)
        {
            var post = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        // POST: /Posts/Edit/5
        [HttpPost]
        public IActionResult Edit(Guid id, Post updatedPost, Guid[] categoryIds)
        {
            if (id != updatedPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var post = _context.Posts.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);

                if (post == null)
                {
                    return NotFound();
                }

                // Update the post's categories
                post.Categories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();

                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(updatedPost);
        }

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
            return RedirectToAction(nameof(Index));
        }
    }
}

