// Services/IStripeService.cs
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public interface IStripeService
    {
        Task<Session> CreateCheckoutSessionAsync(List<ShoppingCartItem> cartItems, string successUrl, string cancelUrl, string couponCode);

        Task<Refund> Refund(int orderId);

    }
}