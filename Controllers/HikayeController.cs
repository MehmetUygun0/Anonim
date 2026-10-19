using Anonim.Data;
using Anonim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anonim.Controllers
{
    public class HikayeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HikayeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Hikayeler.Include(h => h.User).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(h => h.Title.Contains(search) || h.Text.Contains(search));
            }

            var hikayeler = await query
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            ViewBag.Search = search;
            return View(hikayeler);
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
        public async Task<IActionResult> Create(Hikayeler hikaye)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "User");
            }

            hikaye.UserId = currentUserId.Value;
            ModelState.Remove(nameof(Hikayeler.User));

            if (ModelState.IsValid)
            {
                hikaye.CreatedAt = DateTime.Now;
                _context.Hikayeler.Add(hikaye);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(hikaye);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var hikaye = await _context.Hikayeler.FindAsync(id);
            if (hikaye != null)
            {
                if (currentUserId != null && hikaye.UserId == currentUserId.Value)
                {
                    _context.Hikayeler.Remove(hikaye);
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

            var hikaye = await _context.Hikayeler.FindAsync(id);
            if (hikaye == null)
            {
                return Json(new { success = false, message = "Hikaye bulunamadı." });
            }

            var existingLike = await _context.HikayeBegeniler
                .FirstOrDefaultAsync(b => b.HikayeId == id && b.UserId == currentUserId.Value);

            bool isLiked;
            if (existingLike != null)
            {
                _context.HikayeBegeniler.Remove(existingLike);
                hikaye.LikeCount = Math.Max(0, hikaye.LikeCount - 1);
                isLiked = false;
            }
            else
            {
                _context.HikayeBegeniler.Add(new HikayeBegeni
                {
                    HikayeId = id,
                    UserId = currentUserId.Value,
                    CreatedAt = DateTime.Now
                });
                hikaye.LikeCount += 1;
                isLiked = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, liked = isLiked, count = hikaye.LikeCount });
        }
    }
}
