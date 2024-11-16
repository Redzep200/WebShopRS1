using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.SignalR;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;
using Microsoft.AspNetCore.SignalR;
using WebShopSportskeOpreme.Hubs;
using Microsoft.AspNetCore.Authorization;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerQuestionController : ControllerBase
    {
        private readonly ICustomerQuestionService _customerQuestionService;
        private readonly IHubContext<CustomerSupportHub> _hubContext;
        public CustomerQuestionController (ICustomerQuestionService customerQuestionService, IHubContext<CustomerSupportHub> hubContext)
        {
            _customerQuestionService = customerQuestionService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetAllCustomerQuestions() 
        {
            var Helper = _customerQuestionService.GetAllCustomerQuestions();
            if(Helper == null) { return BadRequest("Nema nikakvnih pitanja"); }
            return Ok(Helper);
        }

        [HttpGet]
        public IActionResult GetCustomerQuestionById(int id) 
        {
            var Helper = _customerQuestionService.GetCustomerQuestionById(id);
            if(Helper == null) { return NotFound("Pitanje pod tim ID ne postoji") ; }
            return Ok(Helper);
        }

        [HttpGet]
        public IActionResult GetCustomerQuestionByUserId(int id)
        {
            var Helper = _customerQuestionService.GetCustomerQuestionByUserId(id);
            if (Helper == null) { return NotFound("Taj korisnik nema pitanja"); }
            return Ok(Helper);
        }


        [HttpGet]
        public IActionResult GetCustomerQuestionByUsername(string username)
        {
            var Helper = _customerQuestionService.GetCustomerQuestionByUsername(username);
            if (Helper == null) { return NotFound("Taj korisnik nema pitanja"); }
            return Ok(Helper);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerQuestion([FromBody] AddCustomerQuestionVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerQuestion = new CustomerQuestion
            {
                Text = model.Text,
                UserId = model.UserId,
                QuestionDate = DateTime.Now,
                Closed = false
            };

            var newCQ = _customerQuestionService.CreateCustomerQuestion(customerQuestion);

            if (newCQ == null)
            {
                return BadRequest("Greška pri slanju pitanja.");
            }

            var messageData = new
            {
                Id = newCQ.Id,
                Text = newCQ.Text,
                UserId = newCQ.UserId,
                Closed = newCQ.Closed
            };

            await _hubContext.Clients.All.SendAsync("ReceiveNewMessage", messageData);

            return Ok(new { success = true, message = "Pitanje uspješno poslano." });
        }


        [HttpDelete]
             public async Task<IActionResult> DeleteCustomerQuestion(int id)
        {
            var deletedCQ = _customerQuestionService.DeleteCustomerQuestion(id);
            if (deletedCQ == false) return NotFound("To pitanje ne postoji");
            await _hubContext.Clients.All.SendAsync("QuestionDeleted", id);
            return Ok(new { success = true, message = "Uspjesno obrisano pitanje" });
        }

        [HttpPut]
        public async Task<IActionResult> CreateAnswer([FromBody] AddCustomerAnswerVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = _customerQuestionService.CreateCustomerQuestionAnswer(model.QuestionId, new CustomerQuestion
            {
                Answer = model.Answer,
                Closed = true,
                AnswerDate = DateTime.Now
            });

            if (!status)
                return NotFound("Pitanje ne postoji!");

            var question = _customerQuestionService.GetCustomerQuestionById(model.QuestionId);

            return Ok(new { success = true, message = "Odgovor uspješno poslan" });
        }

    }
}
