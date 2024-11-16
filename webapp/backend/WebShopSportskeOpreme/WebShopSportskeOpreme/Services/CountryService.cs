using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class CountryService : ICountryService
    {
        private readonly WebShopDbContext _context;
        public CountryService(WebShopDbContext context)
        {
            _context = context;
        }
        public Country GetCountryById(int id)
        {
            var country = _context.Countries.FirstOrDefault(c=>c.Id == id);
            return country;
        }
        public Country GetCountryByName(string name)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Name == name);
            return country;
        }

        public List<Country> GetAllCountries()
        {
            var countries = _context.Countries.ToList();
            return countries;
        }
        public Country CreateCountry(Country country)
        {
            if (country == null)
                return null;
            country.Id = 0;
            _context.Countries.Add(country);
            _context.SaveChanges();
            return country;
        }
        public bool UpdateCountry(int id, Country country)
        {
            var newCountry = _context.Countries.FirstOrDefault(c => c.Id == id);
            if (country == null || newCountry == null)
                return false;

            newCountry.Name = country.Name;
            _context.Countries.Update(newCountry);
            _context.SaveChanges();
            return true;
        }
        public bool DeleteCountry(int id)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
                return false;
            _context.Countries.Remove(country);
            _context.SaveChanges();
            return true;
        }

        public Country GetOrCreateCountryByName(string name)
        {
            var country = _context.Countries.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (country == null)
            {
                country = new Country { Name = name };
                _context.Countries.Add(country);
                _context.SaveChanges();
            }

            return country;
        }
    }
}
