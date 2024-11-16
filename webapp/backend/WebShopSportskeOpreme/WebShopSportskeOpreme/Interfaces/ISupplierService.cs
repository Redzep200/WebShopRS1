using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ISupplierService
    {
        Supplier GetSupplierById(int id);
        List<Supplier> GetAllSuppliers();
        Supplier GetSupplierByName(string name);
        bool CreateSupplier(Supplier supplier);
        bool UpdateSupplier(int id,Supplier supplier);
        bool DeleteSupplier(int id);
        List<Supplier> GetSuppliersByCity(int cityId);
    }
}
