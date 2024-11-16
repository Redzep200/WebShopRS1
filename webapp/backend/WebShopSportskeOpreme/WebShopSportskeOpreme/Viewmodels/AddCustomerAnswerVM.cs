using Stripe;
using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCustomerAnswerVM
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
