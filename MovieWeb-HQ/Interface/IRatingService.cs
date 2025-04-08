using MovieWeb_HQ.Models;
using System.Threading.Tasks;

namespace MovieWeb_HQ.Interface
{
    public interface IRatingService
    {
        Task AddRatingAsync(Rating rating);
        Task<double> GetAverageRatingAsync(int movieId);

    }
}
