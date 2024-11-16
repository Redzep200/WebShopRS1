using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCouponVM
    {
        [Required]
        public int PercentDiscount { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public string Value { get; set; }    
    }
}
