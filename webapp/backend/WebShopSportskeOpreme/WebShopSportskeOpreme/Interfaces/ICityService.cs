using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICityService
    {
        City GetCityById(int id);
        City GetCityByName(string name);
        List<City> GetAllCities();
        City CreateCity(City city);
        City GetCityByNameAndCountry(string name, int countryId);
        bool UpdateCity(int id, City city);
        bool DeleteCity(int id);
    }
}
