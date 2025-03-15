using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var movies = _movieService.GetAllMovies();
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
            ViewBag.Genres = new SelectList(_movieService.GetAllMovies().Select(m => m.Genre).Distinct(), "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_movieService.GetAllMovies().SelectMany(m => m.Movie_Countries.Select(mc => mc.Country)).Distinct(), "CountryID", "CountryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, List<int> SelectedCountries, IFormFile ThumbnailFile)
        {
            await _movieService.AddMovieAsync(movie, SelectedCountries, ThumbnailFile);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> WatchMovie(int id)
        {
            var movie = await _movieService.GetMovieDetailsAsync(id);
            if (movie == null) return NotFound();

            ViewBag.RelatedMovies = _movieService.GetRelatedMovies(id, movie.GenreID);
            return View(movie);
        }
    }
}
