// Services/StripeService.cs
using WebShopSportskeOpreme.Models;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly ICouponService _couponService;
        private readonly IOrderService _orderService;

        public StripeService(IConfiguration configuration,  ICouponService couponService, IOrderService orderService) 
        {
            _configuration = configuration;
            _couponService = couponService;
            _orderService = orderService;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Session> CreateCheckoutSessionAsync(List<ShoppingCartItem> cartItems, string successUrl, string cancelUrl, string couponCode = null)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var item in cartItems)
            {
                var cartItem = CartItem.FromShoppingCartItem(item);
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = cartItem.UnitAmount,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cartItem.Name,
                            Metadata = new Dictionary<string, string>
                {
                    { "ProductId", item.ProductId.ToString() }
                }
                        },
                    },
                    Quantity = cartItem.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    { "ShoppingCartId", cartItems.First().ShoppingCartId.ToString() }
                }
            };

            
            if (!string.IsNullOrEmpty(couponCode))
            {
                var couponId = await RetrieveStripeCoupon(couponCode);
                if (!string.IsNullOrEmpty(couponId))
                {
                    options.Discounts = new List<SessionDiscountOptions>
                    {
                        new SessionDiscountOptions
                        {
                            Coupon = couponId,
                        }
                    };
                }
            }

            var service = new SessionService();
            return await service.CreateAsync(options);
        }

        private async Task<string> RetrieveStripeCoupon(string couponCode)
        {
            
            try
            {               
                var stripeCoupon = _couponService.GetCouponByValue(couponCode);
                return stripeCoupon.Id.ToString();
            }
            catch (StripeException ex)
            {               
                if (ex.StripeError?.Type == "invalid_request_error" && ex.StripeError?.Code == "resource_missing")
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<Refund> Refund(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = order.PaymentIntentId,
            };
           
            var refundService = new RefundService();
            Refund refund = await refundService.CreateAsync(refundOptions);
           
            if (refund.Status == "succeeded")
            {
                _orderService.UpdateOrderState(orderId, OrderState.Refunded);
            }

            return refund;
        }

    }
}