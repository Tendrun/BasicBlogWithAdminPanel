using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
