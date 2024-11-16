using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddPromotionsProductsVM
    {
        [Required]
        public int PromotionID { get; set; }
        [Required]
        public int ProductID { get; set; }
    }
}
