using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BasicBlogWithAdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        //  REGISTER
        // ──────────────────────────────────────────────────────────────────────────────
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (!ModelState.IsValid) return View();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Role = UserRole.User
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                // ✅ Set TempData flag
                TempData["ShowWelcomePopup"] = true;

                // ✅ Redirect to user dashboard
                return RedirectToAction("Index", "User");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View();
        }

        // ──────────────────────────────────────────────────────────────────────────────
        //  LOGIN
        // ──────────────────────────────────────────────────────────────────────────────
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                             user.UserName!, password,
                             isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }

            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("Username", user.UserName!);

            return user.Role == UserRole.Admin
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "User");
        }
    }
}
