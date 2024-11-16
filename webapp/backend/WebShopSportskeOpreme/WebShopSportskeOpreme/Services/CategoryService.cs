using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WebShopDbContext _context;

        public CategoryService(WebShopDbContext context)
        {
            _context = context;
        }

        public bool CreateCategory(Category category)
        {
            if (category == null)
                return false;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var country = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (country == null) return false;
            _context.Categories.Remove(country);
            _context.SaveChanges();
            return true;
        }

        public List<Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public Category GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c =>c.Id == id);
            return category;
        }

        public Category GetCategoryByName(string name)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Name == name);
            return category;
        }

        public bool UpdateCategory(int id, Category category)
        {
            var newCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null || category == null) return false;
            newCategory.Name = category.Name;
            newCategory.Description = category.Description;
            _context.Categories.Update(newCategory);
            _context.SaveChanges();
            return true;

        }
    }
}
