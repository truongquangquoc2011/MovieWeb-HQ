using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;
using MovieWeb_HQ.Services;

namespace MovieWeb_HQ.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies().OrderBy(x => Guid.NewGuid()).Take(4).ToList();
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


    }
}
