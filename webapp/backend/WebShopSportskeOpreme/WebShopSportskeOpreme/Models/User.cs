using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Models
{
    public class User : ISoftDeletableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Activity { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string Token { get; set; }
        public string? VerificationToken { get; set; }
    }
}
