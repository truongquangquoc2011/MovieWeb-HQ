using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWeb_HQ.Models
{
    public class WatchHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieID { get; set; }
        public DateTime WatchedAt { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
