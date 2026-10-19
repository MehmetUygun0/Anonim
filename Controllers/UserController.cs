using Anonim.Data;
using Anonim.Helpers;
using Anonim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anonim.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Users user)
        {
            if (ModelState.IsValid)
            {
                bool userExists = await _context.Users.AnyAsync(u => u.NickName == user.NickName);
                if (userExists)
                {
                    ModelState.AddModelError("NickName", "Bu kullanıcı adı zaten alınmış.");
                    return View(user);
                }

                user.Password = PasswordHelper.HashPassword(user.Password);
                user.CreatedAt = DateTime.Now;
                user.LastLogin = DateTime.Now;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nickName, string password)
        {
            if (string.IsNullOrWhiteSpace(nickName) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre zorunludur.";
                return View();
            }

            var hashedPassword = PasswordHelper.HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName && (u.Password == hashedPassword || u.Password == password));
            if (user == null)
            {
                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }

            // Eski düz metin şifre varsa giriş anında otomatik olarak hash'e yükselt
            if (user.Password == password && user.Password != hashedPassword)
            {
                user.Password = hashedPassword;
            }

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            // Session'a kullanıcı bilgilerini kaydet
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("NickName", user.NickName);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
