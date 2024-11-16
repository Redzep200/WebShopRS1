using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSportskeOpreme.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public List<StoreSupplier> StoreSuppliers { get; set; }

        [NotMapped]
        public List<int> SupplierIds { get; set; }
    }
}
