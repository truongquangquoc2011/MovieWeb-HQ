using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách phim
        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Movie_Countries)
                .ThenInclude(mc => mc.Country)
                .ToList();

            return View(movies);
        }

        // Trang chi tiết phim
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        // Phương thức GET để hiển thị form thêm phim
        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres, "GenreID", "GenreName");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryID", "CountryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie, List<int> SelectedCountries, IFormFile ThumbnailFile)
        {
            // Kiểm tra tính hợp lệ của dữ liệu


            // Xử lý upload ảnh
            if (ThumbnailFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ThumbnailFile.FileName); // Đặt tên file duy nhất
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ThumbnailFile.CopyTo(stream);
                }

                movie.Thumbnail = "/uploads/" + fileName;
            }

            // Thêm phim vào database trước để có MovieID
            _context.Movies.Add(movie);
            _context.SaveChanges();

            // Thêm quốc gia cho phim
            if (SelectedCountries != null && SelectedCountries.Any())
            {
                var movieCountries = SelectedCountries.Select(countryId => new Movie_Country
                {
                    MovieID = movie.MovieID, // Lúc này MovieID đã có
                    CountryID = countryId
                });

                _context.Movie_Countries.AddRange(movieCountries);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        // mở trang xem phim 
        public IActionResult WatchMovie(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Lấy danh sách phim liên quan theo thể loại (trừ phim đang xem)
            var relatedMovies = _context.Movies
                .Where(m => m.GenreID == movie.GenreID && m.MovieID != id)
                .Take(5)
                .ToList();

            ViewBag.RelatedMovies = relatedMovies;

            return View(movie);
        }



    }
}



