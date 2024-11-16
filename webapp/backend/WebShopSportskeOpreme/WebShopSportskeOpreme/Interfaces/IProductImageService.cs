using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IProductImageService
    {
        bool CreateProductImage(int productId, string image);
        bool DeleteProductImageByProductId(int productId);
        List<ProductImage> GetAllProductImages();
        List<ProductImage> GetImagesByProductId(int productId);
        List<ProductImage> GetProductImagesByCategoryId(int categoryId);
        List<ProductImage> GetProductImagesByProductName(string name);
    }
}
