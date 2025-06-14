using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult Set(string culture, string returnUrl = "/")
        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture, culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }

            return LocalRedirect(returnUrl);
        }
    }
}
