using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class CityService : ICityService
    {
        private readonly WebShopDbContext _context;
        private readonly ICountryService _countryService;
        public CityService(WebShopDbContext context, ICountryService countryService)
        {
            _context = context;
            _countryService = countryService;
        }
        public City CreateCity(City city)
        {
            if (city == null || _countryService.GetCountryById(city.CountryId) == null)
                return null;

            var existingCity = _context.Cities
                .FirstOrDefault(c => c.Name.ToLower() == city.Name.ToLower() && c.CountryId == city.CountryId);

            if (existingCity != null)
                return existingCity;

            if (string.IsNullOrWhiteSpace(city.ZipCode))
            {
                city.ZipCode = "00000";
            }

            _context.Cities.Add(city);
            _context.SaveChanges();

            city.Country = _countryService.GetCountryById(city.CountryId);
            return city;
        }

        public bool DeleteCity(int id)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
                return false;
            _context.Cities.Remove(city);
            _context.SaveChanges();
            return true;
        }

        public List<City> GetAllCities()
        {
            var cities = _context.Cities.ToList();
            foreach (var item in cities)
                item.Country = _countryService.GetCountryById(item.CountryId);
            
            return cities;
        }

        public City GetCityById(int id)
        {
            var city = _context.Cities.FirstOrDefault(x => x.Id == id);
            if(city != null)
                city.Country = _countryService.GetCountryById(city.CountryId);
            return city;
        }

        public City GetCityByName(string name)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Name == name);
            if (city != null)
                city.Country = _countryService.GetCountryById(city.CountryId);
            return city;
        }

        public bool UpdateCity(int id, City city)
        {
            var newCity = GetCityById(id);
            if (city == null || newCity == null || _countryService.GetCountryById(city.CountryId) == null)
                return false;
            newCity.Name = city.Name;
            newCity.CountryId = city.CountryId;
            newCity.Country = _countryService.GetCountryById(newCity.CountryId);
            newCity.ZipCode = city.ZipCode;
            _context.Cities.Update(newCity);
            _context.SaveChanges();
            return true;
        }

        public City GetCityByNameAndCountry(string name, int countryId)
        {
            var city = _context.Cities
                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower() && c.CountryId == countryId);

            if (city != null)
                city.Country = _countryService.GetCountryById(city.CountryId);

            return city;
        }
    }
}
