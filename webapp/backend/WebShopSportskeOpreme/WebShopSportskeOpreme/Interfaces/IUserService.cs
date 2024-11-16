using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IUserService
    {
        User GetUserById(int id);
        User GetUserByName(string name);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        List<User> GetAllUsers();
        bool CreateUser(User user);
        bool UpdateUser(int id, User user);
        bool DeleteUser(int id);
        User GetUserForAuthentication(string email, string password);
        bool VerifyUser(string token);
    }
}
