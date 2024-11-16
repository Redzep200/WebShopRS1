namespace WebShopSportskeOpreme.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int ReviewRating { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public User? User { get; set; }
        public Product? Product { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
