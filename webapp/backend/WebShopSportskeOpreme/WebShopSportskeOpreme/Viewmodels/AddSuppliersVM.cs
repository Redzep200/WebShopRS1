using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddSuppliersVM
    {
        [Required]
        public int CityId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string ContactPhone { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
