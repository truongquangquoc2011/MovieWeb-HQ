using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }

        public Genre GetGenreById(int id)
        {
            return _context.Genres.Find(id);
        }

        public void AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();
        }

        public string DeleteGenre(int id)
        {
            var genre = _context.Genres
                         .Include(g => g.Movies)
                         .FirstOrDefault(g => g.GenreID == id);

            if (genre == null) return "Thể loại không tồn tại.";

            if (genre.Movies.Any()) // Kiểm tra nếu còn phim
            {
                return "Không thể xóa! Vui lòng xóa các phim thuộc thể loại này trước.";
            }

            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return "Xóa thể loại thành công.";
        }
    }

}
