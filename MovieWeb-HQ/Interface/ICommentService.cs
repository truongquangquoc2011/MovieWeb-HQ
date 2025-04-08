using MovieWeb_HQ.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieWeb_HQ.Interface
{
    public interface ICommentService
    {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByMovieIdAsync(int movieId);
    }
}
