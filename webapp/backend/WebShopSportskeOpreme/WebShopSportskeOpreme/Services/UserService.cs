using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebShopSportskeOpreme.Hubs;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class UserService : IUserService
    {
        private readonly WebShopDbContext _context;
        private readonly ICityService _cityService;
        private readonly IRoleService _roleService;
        private readonly IHubContext<AdminHub> _adminHubContext;
        private readonly IEmailService _emailService;
        public UserService(WebShopDbContext context, ICityService cityService, IRoleService roleService, IHubContext<AdminHub> adminHubContext, IEmailService emailService)
        {
            _context = context;
            _cityService = cityService;
            _roleService = roleService;
            _adminHubContext = adminHubContext;
            _emailService = emailService;
        }

        public bool CreateUser(User user)
        {
            if (user == null || _roleService.GetRoleById(user.RoleId) == null)
                return false;

            var role = _roleService.GetRoleByName("Customer");
            user.Password = HashPassword(user.Password);
            user.IsDeleted = false;
            user.Activity = false;
            user.RoleId = role.Id;
            user.VerificationToken = GenerateVerificationToken();
            _context.Users.Add(user);
            _context.SaveChanges();

            SendVerificationEmail(user);

            return true;
        }

        private string GenerateVerificationToken()
        {
            return Guid.NewGuid().ToString();
        }

        private void SendVerificationEmail(User user)
        {
            string verificationLink = $"http://localhost:4200/verify-email?token={user.VerificationToken}";
            string emailBody = $"Molimo kliknite slijedeci link za verifikaciju: {verificationLink}";
            _emailService.SendEmail(user.Email, "Da li ste to vi", emailBody);
        }

        public bool VerifyUser(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.VerificationToken == token);
            if (user == null)
                return false;

            user.Activity = true;
            user.VerificationToken = null;
            _context.SaveChanges();

            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;
            if (!user.IsDeleted)
            {
                user.IsDeleted = true;
                user.DeletionDate = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            return true;
        }

        public List<User> GetAllUsers()
        {
            var users = _context.Users.Where(u => !u.IsDeleted).ToList();
            foreach (var item in users)
            {
                item.Role = _roleService.GetRoleById(item.RoleId);
            }
            return users;
        }

        public User GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (user != null)
            {
                user.Role = _roleService.GetRoleById(user.RoleId);
            }
            return user;
        }

        public User GetUserByName(string name)
        {
            var user = _context.Users.FirstOrDefault(x => x.Name == name && !x.IsDeleted);
            if (user != null)
            {
                user.Role = _roleService.GetRoleById(user.RoleId);
            }
            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == username && !x.IsDeleted);
            if (user != null)
            {
                user.Role = _roleService.GetRoleById(user.RoleId);
            }
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && !x.IsDeleted);
            if (user != null)
            {
                user.Role = _roleService.GetRoleById(user.RoleId);
            }
            return user;
        }
        public bool UpdateUser(int id, User user)
        {
            var updatedUser = GetUserById(id);
            if (user == null || updatedUser == null
                || _roleService.GetRoleById(user.RoleId) == null)
                return false;
            var roleChanged = updatedUser.RoleId != user.RoleId;
            updatedUser.Name = user.Name;
            updatedUser.Email = user.Email;
            updatedUser.Surname = user.Surname;
            updatedUser.Activity = user.Activity;
            updatedUser.RoleId = user.RoleId;
            updatedUser.Username = user.Username;
            if (user.Password != updatedUser.Password)
            {
                updatedUser.Password = HashPassword(user.Password);
            }
            updatedUser.IsDeleted = user.IsDeleted;
            _context.Users.Update(updatedUser);
            _context.SaveChanges();

            if (roleChanged)
            {
                _adminHubContext.Clients.All.SendAsync("UserRoleChanged", id, user.RoleId);
            }

            return true;
        }

        public User GetUserForAuthentication(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == HashPassword(password));
            return user;
        }
        public string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var passwordInBytes = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(passwordInBytes);

            return Convert.ToBase64String(hashedPassword);
        }
    }
}
