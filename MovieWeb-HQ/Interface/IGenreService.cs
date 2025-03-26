using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Interface
{
    public interface IGenreService
    {
        IEnumerable<Genre> GetAllGenres();
        Genre GetGenreById(int id);
        void AddGenre(Genre genre);
        void UpdateGenre(Genre genre);
        string DeleteGenre(int id);
    }

}
