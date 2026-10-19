using Anonim.Data;
using Anonim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anonim.Controllers
{
    public class ItirafController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItirafController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Itiraflar.Include(i => i.User).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.User.NickName.Contains(search) || i.Title.Contains(search));
            }

            var itiraflar = await query
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            ViewBag.Search = search;
            return View(itiraflar);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Itiraf itiraf)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "User");
            }

            itiraf.UserId = currentUserId.Value;
            ModelState.Remove(nameof(Itiraf.User));

            if (ModelState.IsValid)
            {
                itiraf.CreatedAt = DateTime.Now;
                _context.Itiraflar.Add(itiraf);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(itiraf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var itiraf = await _context.Itiraflar.FindAsync(id);
            if (itiraf != null)
            {
                if (currentUserId != null && itiraf.UserId == currentUserId.Value)
                {
                    _context.Itiraflar.Remove(itiraf);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return Json(new { success = false, message = "Beğenmek için lütfen önce giriş yapın.", requireLogin = true });
            }

            var itiraf = await _context.Itiraflar.FindAsync(id);
            if (itiraf == null)
            {
                return Json(new { success = false, message = "İtiraf bulunamadı." });
            }

            var existingLike = await _context.ItirafBegeniler
                .FirstOrDefaultAsync(b => b.ItirafId == id && b.UserId == currentUserId.Value);

            bool isLiked;
            if (existingLike != null)
            {
                _context.ItirafBegeniler.Remove(existingLike);
                itiraf.LikeCount = Math.Max(0, itiraf.LikeCount - 1);
                isLiked = false;
            }
            else
            {
                _context.ItirafBegeniler.Add(new ItirafBegeni
                {
                    ItirafId = id,
                    UserId = currentUserId.Value,
                    CreatedAt = DateTime.Now
                });
                itiraf.LikeCount += 1;
                isLiked = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, liked = isLiked, count = itiraf.LikeCount });
        }
    }
}
