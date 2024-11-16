using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICountryService
    {
        Country GetCountryById(int id);
        Country GetCountryByName(string name);
        List<Country> GetAllCountries();
        Country CreateCountry(Country country);
        bool UpdateCountry(int id, Country country);
        bool DeleteCountry(int id);
        Country GetOrCreateCountryByName(string name);

    }
}
