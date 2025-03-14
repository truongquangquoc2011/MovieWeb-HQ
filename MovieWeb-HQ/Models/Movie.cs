using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWeb_HQ.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public int GenreID { get; set; }
        public Genre Genre { get; set; }

        public double IMDB_Rating { get; set; }

        public string Thumbnail { get; set; }  // Link ảnh
        public string VideoURL { get; set; }   // Link video
        public string TrailerURL { get; set; }   // Link trailer 

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Movie_Country> Movie_Countries { get; set; }


    }

}
