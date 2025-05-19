using BasicBlogWithAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BasicBlogWithAdminPanel.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Account/Register
        public IActionResult Register() {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password) {
            if (ModelState.IsValid) {
                var user = new ApplicationUser {
                    UserName = email,
                    Email = email,
                    Role = UserRole.User // lub Admin
                };

                Debug.WriteLine("Przed Tutaj ");
                Debug.WriteLine($"email: {email}");
                Debug.WriteLine($"password: {password}");

                var result = await _userManager.CreateAsync(user, password);

                Debug.WriteLine("Tutaj " + result);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        // GET: Account/Login
        public IActionResult Login() {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null) {
                // zapisanie danych do sesji

                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded) {
                    HttpContext.Session.SetString("UserRole", user.Role.ToString());
                    HttpContext.Session.SetString("Username", user.UserName);

                    if (user.Role == UserRole.Admin) {
                        return RedirectToAction("Dashboard", "Admin");
                    } else {
                        return RedirectToAction("Index", "User");
                    }
                }

                ViewBag.Error = "Nieprawidłowy login lub hasło.";
                return View();
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
        }
    }
}
