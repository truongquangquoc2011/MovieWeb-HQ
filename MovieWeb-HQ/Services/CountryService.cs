using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Services
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _context;

        public CountryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.Find(id);
        }

        public void AddCountry(Country country)
        {
            _context.Countries.Add(country);
            _context.SaveChanges();
        }

        public void UpdateCountry(Country country)
        {
            _context.Countries.Update(country);
            _context.SaveChanges();
        }

        public string DeleteCountry(int id)
        {
            var country = _context.Countries
                                  .Include(c => c.Movie_Countries)
                                  .FirstOrDefault(c => c.CountryID == id);

            if (country == null) return "Quốc gia không tồn tại.";

            if (country.Movie_Countries.Any()) // Kiểm tra xem có phim nào liên quan không
            {
                return "Không thể xóa! Vui lòng xóa các phim thuộc quốc gia này trước.";
            }

            _context.Countries.Remove(country);
            _context.SaveChanges();
            return "Xóa quốc gia thành công.";
        }
    }
}
