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

        // ───────── LIST ─────────
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role is null)
                return Unauthorized("You must be logged in.");

            var posts = await _db.Posts
                                 .Where(p => !p.IsDeleted)
                                 .Include(p => p.Comments.OrderBy(c => c.CreatedAt))
                                 .OrderByDescending(p => p.CreatedAt)
                                 .ToListAsync();

            ViewData["UserRole"] = role;
            return View(posts);              // View now expects List<Post>
        }

        // ───────── ADD-COMMENT ─────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest();

            if (!_db.Posts.Any(p => p.Id == postId && !p.IsDeleted))
                return NotFound();

            var comment = new Comment
            {
                PostId = postId,
                Content = content.Trim(),
                Author = User.Identity?.Name ?? "anonymous"
            };
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ───────── DELETE POST (owner or admin) ─────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var post = await _db.Posts.FindAsync(id);
            if (post is null) return NotFound();

            if (role != "Admin" && !post.Author.Equals(User.Identity!.Name, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
