using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicBlogWithAdminPanel.Controllers
{
    [Authorize]                     // must be logged-in
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _um;

        public AdminController(ApplicationDbContext db,
                               UserManager<ApplicationUser> um)
        {
            _db = db;
            _um = um;
        }

        /* -----------------------------------------------------------------
           Helper: is the current authenticated user really an admin?
        ----------------------------------------------------------------- */
        private async Task<bool> CurrentUserIsAdminAsync()
        {
            // ① Identity user
            var user = await _um.GetUserAsync(User);
            if (user is { Role: UserRole.Admin }) return true;

            // ② Fallback: session flag (set during Login action)
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        /* -----------------------------------------------------------------
           GET  /Admin           →  /Admin/Dashboard
           GET  /Admin/Index     →  /Admin/Dashboard
        ----------------------------------------------------------------- */
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index() => RedirectToAction(nameof(Dashboard));

        /* -----------------------------------------------------------------
           GET  /Admin/Dashboard
        ----------------------------------------------------------------- */
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            if (!await CurrentUserIsAdminAsync())
                return Unauthorized();          // 401 for non-admins

            var user = await _um.GetUserAsync(User);

            ViewBag.Username = user?.Email ?? "admin";
            ViewBag.TotalUsers = await _db.Users.CountAsync();
            ViewBag.TotalPosts = await _db.Posts.CountAsync();
            ViewBag.TotalComments = await _db.Comments.CountAsync();

            return View();                      // Views/Admin/Dashboard.cshtml
        }
    }
}
