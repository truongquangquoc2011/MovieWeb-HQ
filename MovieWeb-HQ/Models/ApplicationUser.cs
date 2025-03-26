using Microsoft.AspNetCore.Identity;

namespace MovieWeb_HQ.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
