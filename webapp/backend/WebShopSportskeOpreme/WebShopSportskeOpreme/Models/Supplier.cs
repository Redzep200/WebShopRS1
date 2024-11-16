using System.Reflection.Metadata.Ecma335;

namespace WebShopSportskeOpreme.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public List<StoreSupplier> StoreSuppliers { get; set; }
    }
}
