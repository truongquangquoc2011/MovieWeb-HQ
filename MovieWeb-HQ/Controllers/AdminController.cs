using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;
using MovieWeb_HQ.Services;
namespace MovieWeb_HQ.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMovieService _movieService;
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageMovies()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction("ManageMovies");
        }

      










    }
}
