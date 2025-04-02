using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Controllers
{
    public class WatchHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WatchHistoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveWatchHistory(int movieId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            var watchHistory = await _context.WatchHSs
                .FirstOrDefaultAsync(w => w.MovieID == movieId && w.UserId == user.Id);

            if (watchHistory == null)
            {
                watchHistory = new WatchHS
                {
                    MovieID = movieId,
                    UserId = user.Id,
                    WatchedAt = DateTime.Now
                };
                _context.WatchHSs.Add(watchHistory);
            }
            else
            {
                watchHistory.WatchedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        [Authorize]
        public async Task<IActionResult> WatchHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var watchedMovies = await _context.WatchHSs
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.WatchedAt)
                .Select(w => w.Movie)
                .ToListAsync();

            return View(watchedMovies);
        }

    }
}
