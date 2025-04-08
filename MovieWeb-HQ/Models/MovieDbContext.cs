using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieWeb_HQ.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Movie_Country> Movie_Countries { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<WatchHS> WatchHSs { get; set; }
        public DbSet<Comment> Comments { get; set; }  // Thêm bảng bình luận
        public DbSet<Rating> Ratings { get; set; }    // Thêm bảng đánh giá


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Gọi base để cấu hình Identity
            modelBuilder.Entity<Movie_Country>()
                .HasKey(mc => new { mc.MovieID, mc.CountryID });

            modelBuilder.Entity<Movie_Country>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.Movie_Countries)
                .HasForeignKey(mc => mc.MovieID);

            modelBuilder.Entity<Movie_Country>()
                .HasOne(mc => mc.Country)
                .WithMany(c => c.Movie_Countries)
                .HasForeignKey(mc => mc.CountryID);
            // Cấu hình quan hệ giữa Movie và Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Movie)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MovieID)
                .OnDelete(DeleteBehavior.Cascade); // Khi xóa Movie, xóa luôn Comment

            // Cấu hình quan hệ giữa Movie và Rating
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieID)
                .OnDelete(DeleteBehavior.Cascade); // Khi xóa Movie, xóa luôn Rating
        }
    }
    

}
