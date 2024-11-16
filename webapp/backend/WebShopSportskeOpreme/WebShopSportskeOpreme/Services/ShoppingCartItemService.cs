using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        public readonly WebShopDbContext _context;
        public readonly IProductService _productService;
        public readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartItemService(WebShopDbContext context, IProductService productService, IShoppingCartService shoppingCartService)
        {
            _context = context;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public bool CreateNewShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            if(shoppingCartItem == null || _productService.GetProductById(shoppingCartItem.ProductId)==null || _shoppingCartService.GetShoppingCartById(shoppingCartItem.ShoppingCartId)==null) 
            { return false; }
            _context.ShoppingCartItems.Add(shoppingCartItem);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteShoppingCartItem(int id)
        {
            var DeletedSCI = _context.ShoppingCartItems.FirstOrDefault(x => x.Id == id);
            if (DeletedSCI == null) { return false; }
            _context.ShoppingCartItems.Remove(DeletedSCI);
            _context.SaveChanges(); return true;
        }

        public bool DeleteAllShoppingCartItems(int userId)
        {
            var cartItems = _context.ShoppingCartItems.Where(x => x.ShoppingCart.UserId == userId).ToList();
            if (!cartItems.Any()) { return false; }

            _context.ShoppingCartItems.RemoveRange(cartItems);
            _context.SaveChanges();
            return true;
        }

        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            var AllSCI = _context.ShoppingCartItems.ToList();
            foreach (var item in AllSCI)
            {
                item.Product = _productService.GetProductItemByProductId(item.ProductId);
                item.ShoppingCart = _shoppingCartService.GetShoppingCartById(item.ShoppingCartId);
            }
            return AllSCI;
        }

        public decimal GetProductPrice(int id)
        {
            decimal Helper = _productService.GetProductById(id).Price;
            return Helper;
        }

        public ShoppingCartItem GetShoppingCartItemById(int id)
        {
            var SCI = _context.ShoppingCartItems.FirstOrDefault(s=>s.Id== id);
            if (SCI != null)
            {
                SCI.Product = _productService.GetProductItemByProductId(SCI.ProductId);
                SCI.ShoppingCart = _shoppingCartService.GetShoppingCartById(SCI.ShoppingCartId);
            }
            return SCI;
        }

        public decimal GetTotalPrice(int id, int qty)
        {
            var helper = _productService.GetProductById(id).Price;
            decimal TotalPrice = helper * qty;
            if (helper == null) { return 0; }
            return TotalPrice;
        }

        public bool UpdateShoppingCartItem(int id, ShoppingCartItem shoppingCartItem)
        {
            var UpdatedSCI = _context.ShoppingCartItems.FirstOrDefault(s => s.Id== id);
            if (UpdatedSCI == null || shoppingCartItem==null) { return false; }
            UpdatedSCI.ItemPrice = shoppingCartItem.ItemPrice;
            UpdatedSCI.Quantity = shoppingCartItem.Quantity;
            UpdatedSCI.TotalItemPrice = shoppingCartItem.ItemPrice*shoppingCartItem.Quantity;
            _context.ShoppingCartItems.Update(UpdatedSCI);
            _context.SaveChanges();
            return true;
        }
    }
}
