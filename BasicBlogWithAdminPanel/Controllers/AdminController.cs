﻿using Microsoft.AspNetCore.Mvc;

namespace BasicBlogWithAdminPanel.Controllers {
    public class AdminController : Controller {
        public IActionResult Dashboard() {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
    }
}
