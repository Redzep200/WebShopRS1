using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IPromotionService _promotionService;

        public NotificationController(INotificationService notificationService, IPromotionService promotionService)
        {
            _notificationService = notificationService;
            _promotionService = promotionService;
        }

        [HttpGet]
        public IActionResult GetAllNotifications() 
        {
            var Not = _notificationService.GetAllNotifications();
            return Ok(Not);
        }

        [HttpGet]
        public IActionResult GetNotificationById(int id) 
        {
            var Not = _notificationService.GetNotificationById(id);
            if(Not == null) { return NotFound("Notifikacija nije pronađena"); }
            return Ok(Not);
        }

        [HttpGet]
        public IActionResult GetPromotionStartDate(int id) 
        {
            var helper = _promotionService.GetPromotionById(id);
            var helperDate = helper.StartDate;
            return Ok(helperDate);
        }

        [HttpPost]
        public IActionResult CreateNotification(int promotionid,  string text)
        {
            var DateHelper = _notificationService.GetPromotionStartDate(promotionid);
            var Not = _notificationService.CreateNotification(new Models.Notification { NotificationDate= DateHelper, PromotionId=promotionid,Text=text });
            if (Not == false) return BadRequest("Greška pri dodavanju notifikacije");
            return Ok("Uspješno dodana notifikacija");
        }

        [HttpDelete]
        public IActionResult DeleteNotification(int id) 
        {
            var Not = _notificationService.DeleteNotification(id);
            if (Not == false) return NotFound("Ta notifikacija ne postoji");
            return Ok("Uspješno obrisana notifikacija");
        }

        [HttpPut]
        public IActionResult UpdateNotification(int id,int promotionid, string text) 
        {
            var Not = _notificationService.UpdateNotification(id, new Models.Notification { Text = text,NotificationUpdateTime=DateTime.Now });
            if (Not == false) return NotFound("Ta notifikacija ne postoji");
            return Ok("Uspješno ažurirana notifikacija");
        }
    }
}
