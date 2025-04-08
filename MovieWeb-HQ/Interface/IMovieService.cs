using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Task<Movie?> GetMovieByIdAsync(int id);
        List<Country> GetAllCountries();
        List<Genre> GetAllGenres();

        Task<Movie?> GetMovieDetailsAsync(int id);
        Task AddMovieAsync(Movie movie, List<int> selectedCountries, IFormFile? thumbnailFile);
        List<Movie> GetRelatedMovies(int movieId, int genreId);
        List<Movie> GetMoviesByTitle(string title);
        Task DeleteMovieAsync(int id);
        Task<bool> UpdateMovieAsync(Movie movie, List<int> selectedCountries, IFormFile? thumbnailFile);
        Task<List<int>> GetCountriesByMovieId(int movieId);
        Task<List<Country>> GetAllCountriesAsync();
        List<Movie> FilterMovies(int? genreId, int? countryId);
        Task<List<Movie>> GetTop4HistoryMoviesAsync();


    }
}
