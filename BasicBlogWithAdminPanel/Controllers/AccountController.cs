using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // ───────────────── REGISTER ─────────────────
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Role = UserRole.User
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["RegistrationSuccess"] = true;
                return RedirectToAction(nameof(Login));   // → /Account/Login
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // ───────────────── LOGIN ────────────────────
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, password, isPersistent: false, lockoutOnFailure: false);

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
