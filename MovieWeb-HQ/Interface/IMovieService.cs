using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<Movie?> GetMovieDetailsAsync(int id);
        Task AddMovieAsync(Movie movie, List<int> selectedCountries, IFormFile? thumbnailFile);
        List<Movie> GetRelatedMovies(int movieId, int genreId);
    }
}
