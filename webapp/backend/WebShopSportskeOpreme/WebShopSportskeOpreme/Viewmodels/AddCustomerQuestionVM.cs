using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCustomerQuestionVM
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
