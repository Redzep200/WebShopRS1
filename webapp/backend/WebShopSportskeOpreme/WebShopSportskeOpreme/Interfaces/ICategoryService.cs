using System.Data.Common;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICategoryService
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName (string name);
        List<Category> GetAllCategories();
        bool CreateCategory(Category category);
        bool UpdateCategory(int id, Category category);
        bool DeleteCategory(int id);

    }
}
