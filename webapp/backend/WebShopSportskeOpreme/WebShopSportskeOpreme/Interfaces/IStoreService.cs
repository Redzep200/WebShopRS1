using WebShopSportskeOpreme.DTOs;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IStoreService
    {
        List<StoreDTO> GetAllStores();
        Store GetStoreById(int id);
        bool CreateStore(Store store);
        bool UpdateStore(int id, Store store);
        bool DeleteStore(int id);
        List<Store> GetStoreByName(string storeName);
    }
}