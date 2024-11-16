using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCityVM
    {
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ZipCode { get; set; }
    }
}
