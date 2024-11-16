using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public readonly WebShopDbContext _context;
        public readonly IUserService _userService;

        public ShoppingCartService(WebShopDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public bool CreateShoppingCart(ShoppingCart shoppingCart)
        {
            if(shoppingCart== null || _userService.GetUserById(shoppingCart.UserId)==null) return false;
            _context.ShoppingCarts.Add(shoppingCart);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteShoppingCart(int id)
        {
            var Helper = _context.ShoppingCarts.FirstOrDefault(x => x.Id == id);
            if(Helper==null) return false;
            _context.ShoppingCarts.Remove(Helper);
            _context.SaveChanges();
            return true;
        }

        public List<ShoppingCart> GetAllShoppingCarts()
        {
            var Helper = _context.ShoppingCarts.ToList();
            foreach(var item in Helper)
                item.User = _userService.GetUserById(item.UserId);
            return Helper;
        }

        public DateTime GetSCCreationDate(int id)
        {
            var HelperDate = _context.ShoppingCarts.FirstOrDefault(x=>x.Id== id);
            DateTime Date = HelperDate.CreationDate;
            return Date;
        }

        public ShoppingCart GetShoppingCartById(int id)
        {
            var Helper = _context.ShoppingCarts.FirstOrDefault(x=>x.Id==id);
            if(Helper!=null)
                Helper.User = _userService.GetUserById(Helper.UserId);
            return Helper;
        }

        public ShoppingCart GetShoppingCartByUserId(int userId)
        {
            var shoppingCart = _context.ShoppingCarts.FirstOrDefault(x => x.UserId == userId);
            if (shoppingCart != null)
            {
                shoppingCart.User = _userService.GetUserById(shoppingCart.UserId);
            }
            return shoppingCart;
        }

        public bool UpdateShoppingCart(int id, ShoppingCart shoppingCart)
        {
            var Helper = _context.ShoppingCarts.FirstOrDefault(x => x.Id == id);
            if(Helper==null || shoppingCart==null || _userService.GetUserById(shoppingCart.UserId)==null) return false;
            Helper.CreationDate = shoppingCart.CreationDate;
            Helper.UserId = shoppingCart.UserId;
            Helper.User = _userService.GetUserById(shoppingCart.UserId);
            Helper.LastModifiedDate = DateTime.Now;
            _context.ShoppingCarts.Update(Helper);
            _context.SaveChanges();
            return true;
        }
    }
}
