namespace WebShopSportskeOpreme.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate = new DateTime();
    }
}
