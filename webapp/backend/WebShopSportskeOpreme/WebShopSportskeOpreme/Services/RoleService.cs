using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class RoleService : IRoleService
    {
        private readonly WebShopDbContext _context;
        public RoleService(WebShopDbContext context)
        {
            _context = context;
        }
        public bool CreateRole(Role role)
        {
            if (role == null)
                return false;
            _context.Roles.Add(role);
            _context.SaveChanges();
            return true; 
        }

        public bool DeleteRole(int id)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null) return false;
            _context.Roles.Remove(role);
            _context.SaveChanges() ;
            return true;
        }

        public List<Role> GetAllRoles()
        {
            var role = _context.Roles.ToList();
            return role;
        }

        public Role GetRoleById(int id)
        {
            var role = _context.Roles.FirstOrDefault(X=>X.Id== id);
            return role;
        }

        public Role GetRoleByName(string Name)
        {
           var role = _context.Roles.FirstOrDefault(x=>x.Name == Name);
            return role;
        }

        public bool UpdateRole(int id, Role role)
        {
            var newRole = _context.Roles.FirstOrDefault(x=>x.Id == id); 
            if (newRole == null) return false;
            newRole.Name = role.Name;
            _context.Roles.Update(newRole);
            _context.SaveChanges();
            return true;
        }
    }
}
