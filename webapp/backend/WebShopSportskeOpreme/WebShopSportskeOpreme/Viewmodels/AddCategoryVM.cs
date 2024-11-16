using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCategoryVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
