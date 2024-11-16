using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCountryVM
    {
        [Required]
        public string Name { get; set; }    
    }
}
