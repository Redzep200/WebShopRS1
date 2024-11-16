using WebShopSportskeOpreme.Controllers;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly WebShopDbContext _context;
        private readonly ICityService _cityService;
        public SupplierService(WebShopDbContext context, ICityService cityService)
        {
            _context = context;
            _cityService = cityService;
        }

        public bool CreateSupplier(Supplier supplier)
        {
            if (supplier == null || _cityService.GetCityById(supplier.CityId) == null)
                return false;
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == id);
            if (supplier == null)
                return false;
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return true;
        }

        public List<Supplier> GetAllSuppliers()
        {
            var supplier = _context.Suppliers.ToList();
            foreach(var item in supplier)
                item.City = _cityService.GetCityById(item.CityId);
            return supplier;
        }

        public Supplier GetSupplierById(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(x=>x.Id == id);
            if (supplier != null)
                supplier.City = _cityService.GetCityById(supplier.CityId);
            return supplier;
        }

        public Supplier GetSupplierByName(string name)
        {
            var supplier = _context.Suppliers.FirstOrDefault(x => x.Name == name);
            if (supplier != null)
                supplier.City = _cityService.GetCityById(supplier.CityId);
            return supplier;
        }

        public List<Supplier> GetSuppliersByCity(int cityId)
        {
            var suppliers = _context.Suppliers.Where(s => s.CityId == cityId).ToList();
            foreach (var supplier in suppliers)
            {
                supplier.City = _cityService.GetCityById(supplier.CityId);
            }
            return suppliers;
        }

        public bool UpdateSupplier(int id, Supplier supplier)
        {
            var updatedSupplier = GetSupplierById(id);
            if (supplier == null || updatedSupplier == null || _cityService.GetCityById(supplier.CityId) == null)
                return false;
            updatedSupplier.Name = supplier.Name;
            updatedSupplier.Adress = supplier.Adress;
            updatedSupplier.Email = supplier.Email;
            updatedSupplier.CityId = supplier.CityId;
            updatedSupplier.City = _cityService.GetCityById(updatedSupplier.CityId);
            updatedSupplier.ContactPhone = supplier.ContactPhone;
            updatedSupplier.Adress = supplier.Adress;
            _context.Suppliers.Update(updatedSupplier);
            _context.SaveChanges();
            return true;
        }
    }
}
