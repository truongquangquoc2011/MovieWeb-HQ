using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Movie> GetAllMovies()
        {
            return _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Movie_Countries)
                .ThenInclude(mc => mc.Country)
                .ToList();
        }
        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie?> GetMovieDetailsAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.MovieID == id);
        }
        public async Task AddMovieAsync(Movie movie, List<int> selectedCountries, IFormFile? thumbnailFile)
        {
            if (thumbnailFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(stream);
                }

                movie.Thumbnail = "/uploads/" + fileName;
            }
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            if (selectedCountries != null && selectedCountries.Any())
            {
                var movieCountries = selectedCountries.Select(countryId => new Movie_Country
                {
                    MovieID = movie.MovieID,
                    CountryID = countryId
                });

                _context.Movie_Countries.AddRange(movieCountries);
                await _context.SaveChangesAsync();
            }
        }
        public List<Movie> GetRelatedMovies(int movieId, int genreId)
        {
            return _context.Movies
                .Where(m => m.GenreID == genreId && m.MovieID != movieId)
                .Take(5)
                .ToList();
        }
    }
}
