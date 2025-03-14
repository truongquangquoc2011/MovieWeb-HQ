using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieWeb_HQ.Models
{
    public class Movie_Country
    {
        [Key]
        public int MovieID { get; set; }
        public Movie Movie { get; set; }

        [Key]
        public int CountryID { get; set; }
        public Country Country { get; set; }
    }


}
