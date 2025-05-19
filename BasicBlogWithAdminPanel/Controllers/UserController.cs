using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers {
    public class UserController : Controller {
        public IActionResult Index() {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
    }
}
