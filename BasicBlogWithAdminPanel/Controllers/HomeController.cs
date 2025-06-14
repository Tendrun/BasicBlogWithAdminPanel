using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                ViewBag.LoginStatus = $"Zalogowany użytkownik: {User.Identity!.Name}";
            }
            else
            {
                ViewBag.LoginStatus = "Niezalogowany użytkownik.";
            }

            return View();
        }
    }
}
