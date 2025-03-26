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
                .Include(m => m.Movie_Countries)
                .ThenInclude(mc => mc.Country)
                .FirstOrDefaultAsync(m => m.MovieID == id);
        }
        public List<Country> GetAllCountries()
{
    return _context.Countries.ToList();
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
        public List<Movie> GetMoviesByTitle(string title)
        {
            return _context.Movies
                .Include(m => m.Genre) // Load thông tin thể loại
                .Where(m => m.Title.Contains(title)) // Tìm kiếm theo tên phim
                .ToList();
        }
        public async Task UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }


        public List<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }
        
        public async Task<List<int>> GetCountriesByMovieId(int movieId)
        {
            return await _context.Movie_Countries
                .Where(mc => mc.MovieID == movieId)
                .Select(mc => mc.CountryID)
                .ToListAsync();
        }
        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<bool> UpdateMovieAsync(Movie movie, List<int> selectedCountries, IFormFile? thumbnailFile)
        {
            var existingMovie = await _context.Movies.Include(m => m.Movie_Countries).FirstOrDefaultAsync(m => m.MovieID == movie.MovieID);
            if (existingMovie == null) return false;

            existingMovie.Title = movie.Title;
            existingMovie.GenreID = movie.GenreID;
            existingMovie.Year = movie.Year;
            existingMovie.IMDB_Rating = movie.IMDB_Rating;

            if (thumbnailFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(stream);
                }

                existingMovie.Thumbnail = "/uploads/" + fileName;
            }

            existingMovie.Movie_Countries.Clear();
            existingMovie.Movie_Countries = selectedCountries.Select(cid => new Movie_Country { MovieID = movie.MovieID, CountryID = cid }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }



    
}
}
