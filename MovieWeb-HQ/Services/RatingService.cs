
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWeb_HQ.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Rating rating)
        {
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieID == rating.MovieID && r.UserID == rating.UserID);

            if (existingRating != null)
            {
                existingRating.Stars = rating.Stars;
            }
            else
            {
                _context.Ratings.Add(rating);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingAsync(int movieId)
        {
            var ratings = await _context.Ratings.Where(r => r.MovieID == movieId).ToListAsync();
            return ratings.Any() ? ratings.Average(r => r.Stars) : 0;
        }
    }
}
