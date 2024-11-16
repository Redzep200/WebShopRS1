using System.ComponentModel.DataAnnotations;

namespace WebShopSportskeOpreme.Viewmodels
{
    public class AddCommentVM
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public DateTime? UpdateDate { get; set; }
    }
}
