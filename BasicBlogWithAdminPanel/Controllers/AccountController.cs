using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BasicBlogWithAdminPanel.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
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
                var user = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);

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
            Debug.WriteLine("💬 Otrzymano POST /Account/Login");

            Debug.WriteLine("email " + email + " Password = " + password);

            if (!ModelState.IsValid) {
                Debug.WriteLine("❌ ModelState NIE jest valid!");
                return View();
            }

            Debug.WriteLine("✅ ModelState jest valid, próbuję się zalogować...");

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            Debug.WriteLine(result);

            if (result.Succeeded) {
                Debug.WriteLine("✅ Użytkownik poprawnie zalogowany.");
                return RedirectToAction("Index", "Home");
            } else {
                Debug.WriteLine("❌ Logowanie nieudane.");
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
                return View();
            }
        }


    }
}
