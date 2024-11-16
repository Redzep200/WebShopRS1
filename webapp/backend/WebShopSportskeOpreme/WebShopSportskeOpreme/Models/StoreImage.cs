namespace WebShopSportskeOpreme.Models
{
    public class StoreImage
    {
        public int Id { get; set; }
        public int StoreID { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string ImageFormat { get; set; }
    }
}
