using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IManufacturerService
    {
        Manufacturer GetManufacturerByName(string name);
        Manufacturer GetManufacturerById(int id);
        List<Manufacturer> GetAllManufacturers();
        bool CreateManufacturer(Manufacturer manufacturer);
        bool DeleteManufacturer(int id);
        bool UpdateManufacturer(int id, Manufacturer manufacturer);
    }
}
