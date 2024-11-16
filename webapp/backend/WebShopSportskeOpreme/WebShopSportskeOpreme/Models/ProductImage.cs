namespace WebShopSportskeOpreme.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string ImageFormat  { get; set; }
    }
}
