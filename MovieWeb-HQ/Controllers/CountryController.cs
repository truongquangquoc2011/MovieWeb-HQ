using Microsoft.AspNetCore.Mvc;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;

namespace MovieWeb_HQ.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            var countries = _countryService.GetAllCountries();
            return View(countries);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Country country)
        {
            
                _countryService.AddCountry(country);
                return RedirectToAction("Index");
            
        }

        public IActionResult Edit(int id)
        {
            var country = _countryService.GetCountryById(id);
            if (country == null) return NotFound();
            return View(country);
        }

        [HttpPost]
        public IActionResult Edit(Country country)
        {
            
                _countryService.UpdateCountry(country);
                return RedirectToAction("Index");
            return View(country);
        }

        public IActionResult Delete(int id)
        {
            var country = _countryService.GetCountryById(id);
            if (country == null) return NotFound();
            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            string result = _countryService.DeleteCountry(id);

            if (result != "Xóa quốc gia thành công.")
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = result;
            return RedirectToAction("Index");
        }
    }
}
