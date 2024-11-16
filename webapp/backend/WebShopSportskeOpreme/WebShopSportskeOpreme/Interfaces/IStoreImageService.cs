using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IStoreImageService
    {
        bool CreateStoreImage(int storeId, string image);
        bool DeleteStoreImageByStoreId(int storeId);
        List<StoreImage> GetAllStoreImages();
        List<StoreImage> GetImagesByStoreId(int storeId);
        List<StoreImage> GetStoreImagesByStoreName(string name);
    }
}
