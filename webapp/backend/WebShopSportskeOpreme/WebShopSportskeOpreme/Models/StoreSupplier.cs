namespace WebShopSportskeOpreme.Models
{
    public class StoreSupplier
    {
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
