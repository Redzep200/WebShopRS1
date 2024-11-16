using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface IRoleService
    {
        Role GetRoleById(int id);
        Role GetRoleByName(string Name);
        List<Role> GetAllRoles();
        bool CreateRole(Role role);
        bool UpdateRole(int id, Role role);
        bool DeleteRole(int id);
    }
}
