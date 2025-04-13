using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;
using MovieWeb_HQ.Services;
using System.Security.Claims;

namespace MovieWeb_HQ.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICommentService _commentService;  // Thêm comment service
        private readonly IRatingService _ratingService;    // Thêm rating service
        public HomeController(IMovieService movieService, ICommentService commentService, IRatingService ratingService)
        {
            _movieService = movieService;
            _commentService = commentService; // Gán vào biến private
            _ratingService = ratingService;   
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies().OrderBy(x => Guid.NewGuid()).ToList();
            var historyMovies = _movieService.GetTop4HistoryMoviesAsync().Result;
            ViewBag.HistoryMovies = historyMovies;
            return View(movies);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();
            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(_movieService.GetAllGenres(), "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_movieService.GetAllCountries(), "CountryID", "CountryName"); return View();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, List<int> SelectedCountries, IFormFile ThumbnailFile)
        {
            await _movieService.AddMovieAsync(movie, SelectedCountries, ThumbnailFile);
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> WatchMovie(int id)
        {
            var movie = await _movieService.GetMovieDetailsAsync(id);
            if (movie == null) return NotFound();

            ViewBag.RelatedMovies = _movieService.GetRelatedMovies(id, movie.GenreID);
            return View(movie);
        }
        [HttpGet]
        public IActionResult SearchMovies(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new { success = false, message = "Không có từ khóa tìm kiếm" });
            }

            var movies = _movieService.GetMoviesByTitle(query).Select(m => new
            {
                id = m.MovieID,
                title = m.Title,
                genre = m.Genre?.GenreName,
                year = m.Year,
                imdb = m.IMDB_Rating,
                thumbnail = m.Thumbnail
            }).ToList();

            return Json(movies);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null) return NotFound();

            ViewBag.Genres = new SelectList(_movieService.GetAllGenres(), "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_movieService.GetAllCountries(), "CountryID", "CountryName");
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, List<int> SelectedCountries, IFormFile? ThumbnailFile)
        {
            if (id != movie.MovieID) return NotFound();

            var result = await _movieService.UpdateMovieAsync(movie, SelectedCountries, ThumbnailFile);
            if (!result) return BadRequest();

            return RedirectToAction("ManageMovies", "Admin");

        }
        public IActionResult Movies(int? genreId, int? countryId)
        {
            var movies = _movieService.FilterMovies(genreId, countryId);

            ViewBag.Genres = _movieService.GetAllGenres();
            ViewBag.Countries = _movieService.GetAllCountries();

            return View(movies); // Trả về View Movies.cshtml
        }
        
        public async Task<IActionResult> MoviePlayer(int id)
        {
            var movie = await _movieService.GetMovieDetailsAsync(id);
            if (movie == null) return NotFound();
            var comments = await _commentService.GetCommentsByMovieIdAsync(id);
            var averageRating = await _ratingService.GetAverageRatingAsync(id);
            ViewBag.Comments = comments;
            ViewBag.AverageRating = averageRating;
            bool isAllowed = User.IsInRole("Admin") || User.IsInRole("Member");
            ViewBag.IsAllowed = isAllowed;
            return View(movie);  // Trả về view MoviePlayer.cshtml với thông tin phim

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(int movieId, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = new Comment
            {
                MovieID = movieId,
                UserID = userId,
                Content = content
            };
            await _commentService.AddCommentAsync(comment);
            return RedirectToAction("MoviePlayer", new { id = movieId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRating(int movieId, int stars)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rating = new Rating
            {
                MovieID = movieId,
                UserID = userId,
                Stars = stars
            };
            await _ratingService.AddRatingAsync(rating);
            return RedirectToAction("MoviePlayer", new { id = movieId });
        }


    }
}
