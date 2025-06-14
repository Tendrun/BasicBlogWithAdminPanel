using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        //  CREATE
        // ──────────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "User"
                ? View()
                : Unauthorized("Tylko użytkownicy mogą dodawać posty.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string content)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User")
                return Unauthorized("Only users are allowed to create posts.");

            if (!ModelState.IsValid)
                return View(); // return with validation errors

            var post = new Post
            {
                Title = title,
                Content = content,
                Author = _userManager.GetUserName(User) ?? "anonymous",
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        // ──────────────────────────────────────────────────────────────────────────────
        //  LIST
        // ──────────────────────────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role))
                return Unauthorized("You must be logged in to view posts.");

            var posts = await _context.Posts
                                      .Where(p => !p.IsDeleted)
                                      .OrderByDescending(p => p.CreatedAt)
                                      .ToListAsync();

            ViewData["UserRole"] = role;
            return View(posts);
        }

        // ──────────────────────────────────────────────────────────────────────────────
        //  DELETE  (admin only, hard delete)
        // ──────────────────────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                return Unauthorized("Only admins can delete posts.");

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
