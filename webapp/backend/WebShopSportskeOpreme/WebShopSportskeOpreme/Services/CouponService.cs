using Stripe;
using System.Globalization;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Services
{
    public class CouponService : ICouponService
    {
        private readonly WebShopDbContext _context;
        private readonly string _stripeSecretKey;
        public CouponService(WebShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _stripeSecretKey = configuration["Stripe:SecretKey"];
        }

        public async Task<bool> CreateCoupon(Models.Coupon coupon)
        {
            if (coupon == null)
                return false;
          
            StripeConfiguration.ApiKey = _stripeSecretKey;
                                     
                var stripeCouponOptions = new Stripe.CouponCreateOptions
                {
                    PercentOff = (decimal)coupon.PercentDiscount,
                    Duration = "once",
                    Id = coupon.Value 
                };            

                var stripeCouponService = new Stripe.CouponService();
                var stripeCoupon = await stripeCouponService.CreateAsync(stripeCouponOptions);
               
                if (stripeCoupon != null)
                {
                    _context.Coupons.Add(coupon);
                    await _context.SaveChangesAsync();
                    return true;
                }
                     
            return false;
        }

        //public bool CreateNumberOfCoupons(int numberOfCoupons, int discountPerc, DateTime expirationDate)
        //{
        //    if (numberOfCoupons <= 0 || discountPerc <= 0 || discountPerc >= 100 || expirationDate <= DateTime.Now)
        //        return false;
        //    Random random = new Random();
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    for (int i = 0; i < numberOfCoupons; i++)
        //    {
        //        string value = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray()) + "-" +
        //           new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray()) + "-" +
        //           new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        //        Coupon coupon = new Coupon()
        //        {
        //            ExpirationDate = expirationDate,
        //            IsActive = true,
        //            PercentDiscount = discountPerc,
        //            Value = value
        //        };
        //        var status = CreateCoupon(coupon);
        //        if (status == false)
        //            return false;
        //    }
        //    return true;
        //}

        public async Task<bool> DeleteCoupon(int id)
        {
            var coupon = GetCouponById(id);
            if (coupon == null)
                return false;

            StripeConfiguration.ApiKey = _stripeSecretKey;
            var service = new Stripe.CouponService();
            await service.DeleteAsync(coupon.Value);

            _context.Coupons.Remove(coupon);
            _context.SaveChanges();
            return true;
        }

        public List<Models.Coupon> GetAllCoupons()
        {
            var coupons = _context.Coupons.ToList();
            return coupons;
        }

        public Models.Coupon GetCouponById(int couponId)
        {
            var coupon = _context.Coupons.FirstOrDefault(c=>c.Id == couponId);
            return coupon;
        }

        public Models.Coupon GetCouponByValue(string value)
        {
            var coupon = _context.Coupons.FirstOrDefault(c=>c.Value == value);
            return coupon;
        }

        public bool UpdateCoupon(int id, Models.Coupon coupon)
        {
            var newCoupon = _context.Coupons.FirstOrDefault(c => c.Id == id);
            if (coupon == null || newCoupon == null)
                return false;
            newCoupon.PercentDiscount = coupon.PercentDiscount;
            newCoupon.IsActive = coupon.IsActive;
            newCoupon.ExpirationDate = coupon.ExpirationDate;                
            _context.Coupons.Update(newCoupon);
            _context.SaveChanges();
            return true;
        }

        public (bool isValid, int discountPercentage) ValidateCoupon(string couponCode)
        {
            var coupon = GetCouponByValue(couponCode);
            if (coupon != null && coupon.IsActive && coupon.ExpirationDate > DateTime.UtcNow)
            {
                return (true, coupon.PercentDiscount);
            }
            return (false, 0);
        }
    }
}
