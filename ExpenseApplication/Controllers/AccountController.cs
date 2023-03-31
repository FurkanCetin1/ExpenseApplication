using ExpenseApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExpenseApplication.Data;
using Serilog;

namespace ExpenseApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        MasrafAppContext _context = new MasrafAppContext();

        public AccountController(ILogger<AccountController> _logger)
        {
            this._logger = _logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            this._logger.LogInformation("Log Testing");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı ve şifre alanlarını doldurunuz.");
                return View();
            }

           
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            
            if (user == null || user.Password != password)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre");
                return View();
            }

            
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            switch (user.RoleId)
            {
                case 1: // Çalışan
                    return RedirectToAction("Index", "Home");
                case 2: // Yönetici
                    return RedirectToAction("Privacy", "Home");
                case 3: // Muhasebeci
                    return RedirectToAction("Accountant", "Home");
                case 4: // Admin
                    return RedirectToAction("Admin", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

    }
}
