using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddPromotionVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal DiscountPercentage { get; set; }
    }
}
