using BasicBlogWithAdminPanel.Data;
using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create(Post post)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User")
            {
                return Unauthorized("Tylko użytkownicy mogą dodawać posty.");
            }

            if (ModelState.IsValid)
            {
                post.Author = HttpContext.Session.GetString("Username");
                post.CreatedAt = DateTime.Now;
                _context.Posts.Add(post);
                _context.SaveChanges();

                return RedirectToAction("Index", "User");
            }

            return View(post);
        }
    }
}
