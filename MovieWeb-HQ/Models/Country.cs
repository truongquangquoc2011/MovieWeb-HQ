using System.ComponentModel.DataAnnotations;

namespace MovieWeb_HQ.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }

        public ICollection<Movie_Country> Movie_Countries { get; set; }
    }


}
