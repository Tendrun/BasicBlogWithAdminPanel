using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
