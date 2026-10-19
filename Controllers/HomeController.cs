using Anonim.Data;
using Anonim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Anonim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ItirafCount = await _context.Itiraflar.CountAsync();
            ViewBag.HikayeCount = await _context.Hikayeler.CountAsync();
            ViewBag.UserCount = await _context.Users.CountAsync();

            var itiraflar = await _context.Itiraflar
                .Include(i => i.User)
                .OrderByDescending(i => i.CreatedAt)
                .Take(30)
                .ToListAsync();

            var hikayeler = await _context.Hikayeler
                .Include(h => h.User)
                .OrderByDescending(h => h.CreatedAt)
                .Take(30)
                .ToListAsync();

            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var likedItirafIds = new HashSet<int>();
            var likedHikayeIds = new HashSet<int>();

            if (currentUserId != null)
            {
                likedItirafIds = (await _context.ItirafBegeniler
                    .Where(b => b.UserId == currentUserId.Value)
                    .Select(b => b.ItirafId)
                    .ToListAsync())
                    .ToHashSet();

                likedHikayeIds = (await _context.HikayeBegeniler
                    .Where(b => b.UserId == currentUserId.Value)
                    .Select(b => b.HikayeId)
                    .ToListAsync())
                    .ToHashSet();
            }

            var feed = new List<FeedItemViewModel>();

            feed.AddRange(itiraflar.Select(i => new FeedItemViewModel
            {
                Id = i.Id,
                Type = FeedType.Itiraf,
                Title = i.Title,
                Text = i.Text,
                CreatedAt = i.CreatedAt,
                UserId = i.UserId,
                AuthorNickName = i.User?.NickName ?? "Anonim",
                LikeCount = i.LikeCount,
                IsLikedByCurrentUser = likedItirafIds.Contains(i.Id)
            }));

            feed.AddRange(hikayeler.Select(h => new FeedItemViewModel
            {
                Id = h.Id,
                Type = FeedType.Hikaye,
                Title = h.Title,
                Text = h.Text,
                CreatedAt = h.CreatedAt,
                UserId = h.UserId,
                AuthorNickName = h.User?.NickName ?? "Anonim Yazar",
                LikeCount = h.LikeCount,
                IsLikedByCurrentUser = likedHikayeIds.Contains(h.Id)
            }));

            var sortedFeed = feed.OrderByDescending(x => x.CreatedAt).ToList();

            return View(sortedFeed);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
