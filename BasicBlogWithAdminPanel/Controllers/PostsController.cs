using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _um;

        public PostsController(ApplicationDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _um = um;
        }

        // ───────────────────────── LIST ─────────────────────────
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role is null) return Unauthorized("You must be logged in.");

            var posts = await _db.Posts
                                 .Where(p => !p.IsDeleted)
                                 .Include(p => p.Comments.OrderBy(c => c.CreatedAt))
                                 .OrderByDescending(p => p.CreatedAt)
                                 .ToListAsync();

            ViewData["UserRole"] = role;
            return View(posts);
        }

        // ─────────────────────── CREATE (form) ───────────────────────
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity!.IsAuthenticated)
                return Unauthorized("You must be logged in.");

            return View();                           // Views/Posts/Create.cshtml
        }

        // ─────────────────────── CREATE (submit) ─────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Post input)
        {
            ModelState.Remove("Author");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("IsDeleted");
            ModelState.Remove("Comments");

            if (!ModelState.IsValid) return View(input);

            var post = new Post
            {
                Title = input.Title.Trim(),
                Content = input.Content.Trim(),
                Author = User.Identity!.Name ?? "anonymous",
                CreatedAt = DateTime.UtcNow
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ────────────────── ADD COMMENT ──────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return BadRequest();
            if (!_db.Posts.Any(p => p.Id == postId && !p.IsDeleted)) return NotFound();

            _db.Comments.Add(new Comment
            {
                PostId = postId,
                Content = content.Trim(),
                Author = User.Identity?.Name ?? "anonymous"
            });

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ────────────────── EDIT (form) ──────────────────
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null || post.IsDeleted) return NotFound();

            var role = HttpContext.Session.GetString("UserRole");
            var author = User.Identity?.Name ?? "";

            if (role != "Admin" &&
                !post.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            return View(post);          // Views/Posts/Edit.cshtml
        }

        // ─────────────── EDIT (submit) ───────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Content")] Post input)
        {
            ModelState.Remove("Author");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("IsDeleted");
            ModelState.Remove("Comments");

            if (!ModelState.IsValid) return View(input);

            var post = await _db.Posts.FindAsync(input.Id);
            if (post == null || post.IsDeleted) return NotFound();

            var role = HttpContext.Session.GetString("UserRole");
            var author = User.Identity?.Name ?? "";

            if (role != "Admin" &&
                !post.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            post.Title = input.Title.Trim();
            post.Content = input.Content.Trim();
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ─────────── DELETE (owner or admin) ───────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();

            if (role != "Admin" &&
                !post.Author.Equals(User.Identity!.Name, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            // soft-delete so comments/history survive
            post.IsDeleted = true;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
