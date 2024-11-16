using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize(Roles = "Web shop admin, SistemAdmin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(WebShopDbContext context,IRoleService roleService) 
        {
            _roleService = roleService;
        }
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }
        [HttpGet]
        public IActionResult GetRole(int id) 
        {
            var roles = _roleService.GetRoleById(id);
            if (roles == null)
                return NotFound("Uloga ne postoji!");
            return Ok(roles);
        }

        [HttpGet]
        public IActionResult GetRoleByName(string name)
        {
            var role = _roleService.GetRoleByName(name);
            if (role == null)
                return NotFound("Uloga ne postoji");
            return Ok(role);
        }

        [HttpPut]
        public IActionResult UpdateRole(int id, string name) 
        {
            var role = _roleService.UpdateRole(id,new Role { Name = name});
            if (role == false)
                return NotFound("Uloga ne postoji!");
            return Ok("Uspješno izmjenjena uloga!");
        }
        [HttpDelete]
        public IActionResult DeleteRole(int id) 
        {
           var role = _roleService.DeleteRole(id);
            if (role == false)
                return NotFound("Uloga sa ID-om " + id + " ne postoji!");
            return Ok("Uspješno obrisana uloga!");
        }
        [HttpPost]
        public IActionResult AddRole(string name) 
        {
            var role = _roleService.CreateRole(new Role { Name = name });
            if (role == false)
                return NotFound("Greška pri dodavanju uloge!");
            return Ok("Uspješno dodana uloga " + name + "!");
        }
    }
}
