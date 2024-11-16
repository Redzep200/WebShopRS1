using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet]
        public IActionResult GetAllCoupons()
        {
            var coupons = _couponService.GetAllCoupons();
            return Ok(coupons);
        }

        [HttpGet]
        public IActionResult GetCouponById(int id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (coupon == null)
                return NotFound("Kupon nije pronađen!");
            return Ok(coupon);
        }

        [HttpGet]
        public IActionResult GetCouponByValue(string couponvalue)
        {
            var coupon = _couponService.GetCouponByValue(couponvalue);
            if (coupon == null)
                return NotFound("Kupon nije pronađen!");
            return Ok(coupon);
        }

        [HttpPost]
        public IActionResult ValidateCoupon([FromBody] string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return BadRequest("Coupon code is required.");
            }

            var (isValid, discountPercentage) = _couponService.ValidateCoupon(couponCode);

            if (isValid)
            {
                return Ok(new { valid = true, discount = discountPercentage });
            }
            else
            {
                return Ok(new { valid = false, message = "Invalid or expired coupon." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(AddCouponVM model)
        {
            if (ModelState.IsValid)
            {
                var manufacturer = await _couponService.CreateCoupon(new Coupon
                {
                    PercentDiscount = model.PercentDiscount,
                    ExpirationDate = model.ExpirationDate,
                    Value = model.Value,
                 
                }); 

                if (!manufacturer)
                    return BadRequest("Greška u dodavanju kupona!");

                return Ok(new { success = true, message = "Uspješno dodan kupon" });
            }

            return BadRequest(ModelState);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var status = await _couponService.DeleteCoupon(id);
            if (!status)
                return NotFound(new { success = false, message = "Kupon nije pronađen ili se nije mogao obrisati." });

            return Ok(new { success = true, message = "Uspješno izbrisan kupon!" });
        }

        [HttpPut]
        public IActionResult UpdateCoupon(int id, bool isActive, DateTime expDate, int discountPerc)
        {
            var status = _couponService.UpdateCoupon(id, new Coupon
            { IsActive = isActive,
              PercentDiscount = discountPerc,
              ExpirationDate = expDate});
            if (status == false)
                return NotFound("Kupon nije pronađen!");
            return Ok("Uspješno promijenjen kupon!");
        }

        //[HttpPost]
        //public IActionResult CreateNumberOfCoupons(int numberOfCoupons, int discountPerc, DateTime expirationDate)
        //{
        //    var status = _couponService.CreateNumberOfCoupons(numberOfCoupons, discountPerc, expirationDate);
        //    if (status == false)
        //        return BadRequest("Greška u dodavanju " + numberOfCoupons + " kupona!");
        //    return Ok("Uspješno dodano " + numberOfCoupons + " kupona!");
        //}


    }
}
