using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICouponService
    {
        List<Coupon> GetAllCoupons();
        Coupon GetCouponById(int couponId);
        Coupon GetCouponByValue(string value);
        Task<bool> CreateCoupon(WebShopSportskeOpreme.Models.Coupon coupon);
        bool UpdateCoupon(int id, Coupon coupon);
        Task<bool> DeleteCoupon(int id);
        //bool CreateNumberOfCoupons(int numberOfCoupons, int discountPerc, DateTime expirationDate);
        (bool isValid, int discountPercentage) ValidateCoupon(string couponCode);
    }
}
