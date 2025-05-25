using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User")
            {
                return Unauthorized("Tylko użytkownicy mogą dodawać posty.");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string content) {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "User") {
                return Unauthorized("Only users are allowed to create posts.");
            }

            foreach (var kv in ModelState) {
                foreach (var error in kv.Value.Errors) {
                    Debug.WriteLine($"Error on {kv.Key}: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid) {
                var post = new Post {
                    Title = title,
                    Content = content,
                    Author = _userManager.GetUserId(User),
                    CreatedAt = DateTime.Now
                };

                _context.Posts.Add(post);
                _context.SaveChanges();

                return RedirectToAction("Index", "User");
            }

            // Return to the form with errors
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Index() {
            var role = HttpContext.Session.GetString("UserRole");

            // Optional: redirect if user is not logged in at all
            if (string.IsNullOrEmpty(role)) {
                return Unauthorized("You must be logged in to view posts.");
            }

            var posts = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Pass role to the view using ViewData
            ViewData["UserRole"] = role;

            return View(posts);
        }



        // POST: /Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin") {
                return Unauthorized("Only admins can delete posts.");
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null) {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
