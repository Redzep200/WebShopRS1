using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IProductService
    {
        Product GetProductById(int id);
        List<Product> GetProductByName(string productName);
        List<Product> GetProductByCategory(int id);
        List<Product> GetAllProducts();
        List<ProductItemVM> GetAllProductItems();
        ProductItemVM GetProductItemByProductId(int productId);
        List<ProductItemVM> GetProductItemsByCategoryId(int categoryId);
        List<ProductItemVM> GetProductItemsByProductName(string productName);
        List<ProductItemVM> GetProductItemsByPriceRange(int minPrice, int maxPrice);
        List<ProductItemVM> GetProductItemsByManufacturerId(int manufacturerId);
        List<ProductItemVM> GetFilteredProductItems(string? productName, int? categoryId, int? minPrice, int? maxPrice, int? manufacturerId);
        bool CreateProduct(Product product);
        bool UpdateProduct(int id, Product product);
        bool DeleteProduct(int id);
        bool DeleteProductItem(int productId);
    }
}
