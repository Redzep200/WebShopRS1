namespace WebShopSportskeOpreme.Models
{
    public class CustomerQuestion
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime QuestionDate { get; set; }
        public string? Answer { get; set; }
        public bool Closed { get; set; } = false;
        public DateTime? AnswerDate { get; set; }
        public string? Username { get; set; }
    }
}
