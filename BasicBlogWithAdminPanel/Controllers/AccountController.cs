using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
