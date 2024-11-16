using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using WebShopSportskeOpreme.DTOs;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class StoreService : IStoreService
    {
        private readonly WebShopDbContext _context;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly ISupplierService _supplierService;

        public StoreService(WebShopDbContext context, ICityService cityService, ICountryService countryService, ISupplierService supplierService)
        {
            _context = context;
            _cityService = cityService;
            _countryService = countryService;
            _supplierService = supplierService;
        }

        public List<StoreDTO> GetAllStores()
        {
            return _context.Stores
                .AsNoTracking()
                .Select(s => new StoreDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    CityId = s.CityId,
                    CityName = s.City.Name,
                    CountryName = s.City.Country.Name,
                    Suppliers = s.StoreSuppliers.Select(ss => new SupplierDTO
                    {
                        Id = ss.Supplier.Id,
                        Name = ss.Supplier.Name,
                        Address = ss.Supplier.Adress,
                        ContactPhone = ss.Supplier.ContactPhone,
                        Email = ss.Supplier.Email
                    }).ToList()
                })
                .ToList();
        }

        public Store GetStoreById(int id)
        {
            return _context.Stores
                .Include(s => s.City)
                .Include(s => s.StoreSuppliers)
                .ThenInclude(ss => ss.Supplier)
                .FirstOrDefault(s => s.Id == id);
        }

        public bool CreateStore(Store store)
        {
            if (store == null || string.IsNullOrWhiteSpace(store.Name))
                return false;

            Country country;
            if (store.City.CountryId != 0)
            {
                country = _countryService.GetCountryById(store.City.CountryId);
            }
            else
            {
                country = _countryService.GetCountryByName(store.City.Country.Name);
            }

            if (country == null)
            {
                country = new Country { Name = store.City.Country.Name };
                country = _countryService.CreateCountry(country);
            }

            var city = _cityService.GetCityByNameAndCountry(store.City.Name, country.Id);
            if (city == null)
            {
                city = new City
                {
                    Name = store.City.Name,
                    CountryId = country.Id,
                    ZipCode = store.City.ZipCode ?? "00000" 
                };
                city = _cityService.CreateCity(city);
            }

            store.CityId = city.Id;
            store.City = null; 

            store.StoreSuppliers = store.StoreSuppliers ?? new List<StoreSupplier>();

            if (store.SupplierIds != null && store.SupplierIds.Any())
            {
                foreach (var supplierId in store.SupplierIds)
                {
                    store.StoreSuppliers.Add(new StoreSupplier { SupplierId = supplierId });
                }
            }
            else
            {
                var suppliersInCity = _supplierService.GetSuppliersByCity(city.Id);
                foreach (var supplier in suppliersInCity)
                {
                    store.StoreSuppliers.Add(new StoreSupplier { SupplierId = supplier.Id });
                }
            }

            _context.Stores.Add(store);
            _context.SaveChanges();


            return true;
        }

        public bool UpdateStore(int id, Store store)
        {
            var existingStore = GetStoreById(id);
            if (existingStore == null)
                return false;

            existingStore.Name = store.Name;
            existingStore.Address = store.Address;
            existingStore.Latitude = store.Latitude;
            existingStore.Longitude = store.Longitude;

            var country = _countryService.GetCountryById(store.City.CountryId);
            if (country == null)
            {
                country = new Country { Name = store.City.Country.Name };
                country = _countryService.CreateCountry(country);
            }

            var city = _cityService.GetCityByNameAndCountry(store.City.Name, country.Id);
            if (city == null)
            {
                city = new City
                {
                    Name = store.City.Name,
                    CountryId = country.Id,
                    ZipCode = store.City.ZipCode ?? "00000"
                };
                city = _cityService.CreateCity(city);
            }

            existingStore.CityId = city.Id;

            var existingSuppliers = _context.StoreSuppliers.Where(ss => ss.StoreId == id).ToList();
            _context.StoreSuppliers.RemoveRange(existingSuppliers);
            foreach (var supplierId in store.SupplierIds)
            {
                _context.StoreSuppliers.Add(new StoreSupplier { StoreId = id, SupplierId = supplierId });
            }

            _context.Stores.Update(existingStore);
            _context.SaveChanges();

            return true;
        }


        public bool DeleteStore(int id)
        {
            var store = GetStoreById(id);
            if (store == null)
                return false;

            var storeSuppliers = _context.StoreSuppliers.Where(ss => ss.StoreId == id);
            _context.StoreSuppliers.RemoveRange(storeSuppliers);

            _context.Stores.Remove(store);
            _context.SaveChanges();
            return true;
        }

        public List<Store> GetStoreByName(string storeName)
        {
            var store = _context.Stores.Where(s => s.Name.ToLower().Contains(storeName.ToLower())).ToList();
            return store;
        }
    }
}