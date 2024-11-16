using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly WebShopDbContext _context;
        public ManufacturerService(WebShopDbContext context)
        {
            _context = context;
        }

        public bool CreateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null) return false;
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteManufacturer(int id)
        {
            var manufacturer = _context.Manufacturers.FirstOrDefault(m => m.Id == id);
            if (manufacturer == null) return false;
            _context.Manufacturers.Remove(manufacturer);
            _context.SaveChanges() ;
            return true;
        }

        public List<Manufacturer> GetAllManufacturers()
        {
            var manufacturers = _context.Manufacturers.ToList();
            return manufacturers;
        }

        public Manufacturer GetManufacturerById(int id)
        {
            var manufacturer = _context.Manufacturers.FirstOrDefault(m=>m.Id== id);
            return manufacturer;
        }

        public Manufacturer GetManufacturerByName(string name)
        {
            var manufacturer = _context.Manufacturers.FirstOrDefault(m => m.Name == name);
            return manufacturer;
        }

        public bool UpdateManufacturer(int id, Manufacturer manufacturer)
        {
            var _manufacturer = _context.Manufacturers.FirstOrDefault(m=>m.Id== id);
            if (manufacturer == null || _manufacturer == null) return false;
            _manufacturer.Name = manufacturer.Name;
            _manufacturer.Address = manufacturer.Address;
            _manufacturer.Email = manufacturer.Email;
            _context.Manufacturers.Update(_manufacturer);
            _context.SaveChanges();
            return true;
        }
    }
}
