using Microsoft.AspNetCore.Mvc;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public IActionResult Index()
        {
            var genres = _genreService.GetAllGenres();
            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Genre genre)
        {
           
                _genreService.AddGenre(genre);
                return RedirectToAction("Index");
            return View(genre);
        }

        public IActionResult Edit(int id)
        {
            var genre = _genreService.GetGenreById(id);
            if (genre == null) return NotFound();
            return View(genre);
        }

        [HttpPost]
        public IActionResult Edit(Genre genre)
        {
            
                _genreService.UpdateGenre(genre);
                return RedirectToAction("Index");
            
            return View(genre);
        }

        public IActionResult Delete(int id)
        {
            var genre = _genreService.GetGenreById(id);
            if (genre == null) return NotFound();
            return View(genre);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            string result = _genreService.DeleteGenre(id);

            if (result != "Xóa thể loại thành công.")
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = result;
            return RedirectToAction("Index");
        }

    }

}
