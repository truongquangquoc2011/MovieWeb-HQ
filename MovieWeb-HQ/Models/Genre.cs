using System.ComponentModel.DataAnnotations;

namespace MovieWeb_HQ.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }

        [Required]
        [StringLength(100)]
        public string GenreName { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }

}
