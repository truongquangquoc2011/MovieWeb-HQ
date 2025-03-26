using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Interface
{
    public interface ICountryService
    {
        IEnumerable<Country> GetAllCountries();
        Country GetCountryById(int id);
        void AddCountry(Country country);
        void UpdateCountry(Country country);
        string DeleteCountry(int id); // Trả về thông báo nếu xóa thất bại
    }
}
