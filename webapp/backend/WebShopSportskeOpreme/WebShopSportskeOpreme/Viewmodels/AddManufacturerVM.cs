using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddManufacturerVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email {  get; set; }
    }
}
