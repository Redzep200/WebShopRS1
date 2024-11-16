using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Models.HelperModels;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IShoppingCartService _shoppingCartService;
        public UserController(IUserService userService, IRoleService roleService, IShoppingCartService shoppingCartService)
        {
            _userService = userService;
            _roleService = roleService;
            _shoppingCartService = shoppingCartService;
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin, Support")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpGet]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) { return NotFound("Korisnik kojeg tražite ne postoji!"); }
            return Ok(user);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpGet]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null) { return NotFound("Korisnik kojeg tražite ne postoji!"); }
            return Ok(user);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserVM data)
        {
            if (data == null)
                return BadRequest("Invalid data");

            var existingUser = _userService.GetUserByEmail(data.Email);
            if (existingUser != null)
                return BadRequest("Email je već registrovan.");

            existingUser = _userService.GetUserByUsername(data.Username);
            if (existingUser != null)
                return BadRequest("Korisničko ime je već registrovano.");

            var role = _roleService.GetRoleByName("Customer");
            var user = new User
            {
                Name = data.Name,
                Surname = data.Surname,
                Email = data.Email,
                Password = data.Password,
                Username = data.Username,
                RegistrationDate = DateTime.Now,
                Activity = false,
                Token = "Token",
                IsDeleted = false,
                VerificationToken = null
            };
            var result = _userService.CreateUser(user);
            if (!result)
                return BadRequest("Greška pri kreiranju korisnika");
            return Ok(new { success = true, message = "Uspjesno kreiran korisnik" });
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var user = _userService.DeleteUser(id);
            if (user == false) return BadRequest("Neuspješno brisanje korisnika!");
            return Ok(new { success = true, message = "Uspjesno obrisan korisnik" });
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateUser(int id, int roleId, string name, string surname, string email, string password, bool activity, string username)
        {
           
            var status = _userService.UpdateUser(id, new User
            {
                RoleId = roleId,
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                Activity = activity,
                Username = username
            });
            if (status == false)
                return NotFound("Ne postoji korisnik kojeg želite ažurirati!");
            return Ok(new { success = true, message = "Uspjesno editovan korisnik" });
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginData data)
        {
            if (data == null)
                return BadRequest();
            var userFromService = _userService.GetUserForAuthentication(data.Email, data.Password);
            if (userFromService == null)
                return NotFound("Korisnik nije pronađen!");
            if (userFromService.Activity == false)
                return BadRequest("Korisnik nije verifikovan. Provjerite email");

            userFromService.Role = _roleService.GetRoleById(userFromService.RoleId);

            userFromService.Token = CreateJwt(userFromService);
            return Ok(new
            {
                Message = "Uspjesno logiranje!",
                Token = userFromService.Token,
                Email = userFromService.Email,
                UserId = userFromService.Id,
                Role = userFromService.Role.Name
            });
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] RegisterData data)
        {
            if (data == null)
                return BadRequest();

            var existingUser = _userService.GetUserByEmail(data.Email);
            if (existingUser != null)
                return BadRequest("Email je već registrovan.");

            existingUser = _userService.GetUserByUsername(data.Username);
            if (existingUser != null)
                return BadRequest("Korisničko ime je već registrovano.");

            var user = new User
            {
                Activity = false,
                RoleId = 1,
                RegistrationDate = DateTime.Now,
                Name = data.Name,
                Surname = data.Surname,
                Email = data.Email,
                Username = data.Username,
                Password = data.Password,
                Token = "",
                VerificationToken = null
            };

            var status = _userService.CreateUser(user);
            if (status == false) return BadRequest("Neuspješna registracija!");

            var cartCreated = _shoppingCartService.CreateShoppingCart(new Models.ShoppingCart
            {
                CreationDate = DateTime.Now,
                UserId = user.Id
            });

            if (!cartCreated)
            {
                return BadRequest("Registracija uspješna, ali neuspješno kreiranje korpe.");
            }
            return Ok(new { success = true, message = "Uspjesna registracija, provjerite svoj email" });
        }

        private string CreateJwt(User user)
        {
            var role = _roleService.GetRoleById(user.RoleId);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("VeryCoolWebShopKeyForJWTToken123");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)


            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet]
        public IActionResult VerifyEmail(string token)
        {
            var result = _userService.VerifyUser(token);
            if (result)
                return Ok(new { succes=true, message="Email je uspješno verificiran. Možete se prijaviti na svoj račun." });
            else
                return BadRequest("Nevažeći token za verifikaciju.");
        }
    }
}
