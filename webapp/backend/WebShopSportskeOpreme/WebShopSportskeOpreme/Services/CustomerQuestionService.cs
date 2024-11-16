using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using Microsoft.AspNetCore.SignalR;
using WebShopSportskeOpreme.Hubs;

namespace WebShopSportskeOpreme.Services
{
    public class CustomerQuestionService : ICustomerQuestionService
    {
        public readonly IUserService _userService;
        public readonly WebShopDbContext _context;
        private readonly IHubContext<CustomerSupportHub> _hubContext;
        private readonly IEmailService _emailService;

        public CustomerQuestionService(IUserService userService, WebShopDbContext context, IHubContext<CustomerSupportHub> hubContext, IEmailService emailService)
        {
            _userService = userService;
            _context = context;
            _hubContext = hubContext;
            _emailService = emailService;
        }

        public CustomerQuestion CreateCustomerQuestion(CustomerQuestion customerQuestion)
        {
            if (customerQuestion == null || _userService.GetUserById(customerQuestion.UserId) == null)
            {
                return null;
            }
            _context.CustomerQuestions.Add(customerQuestion);
            _context.SaveChanges();
            return customerQuestion;
        }


        public bool DeleteCustomerQuestion(int id)
        {
            var DelCQ = _context.CustomerQuestions.FirstOrDefault(x => x.Id == id);
            if(DelCQ == null) { return false; }
            _context.CustomerQuestions.Remove(DelCQ);
            _context.SaveChanges();
            return true;
        }

        public List<CustomerQuestion> GetAllCustomerQuestions()
        {
            var ListCQ = _context.CustomerQuestions.ToList();
            foreach(var item in ListCQ)
            {
                item.User = _userService.GetUserById(item.UserId);
            }
            return ListCQ;
        }

        public CustomerQuestion GetCustomerQuestionById(int id)
        {
            var helperCQ = _context.CustomerQuestions.FirstOrDefault(c=>c.Id== id);
            if(helperCQ != null)
            {
                helperCQ.User = _userService.GetUserById(helperCQ.UserId);
            }
            return helperCQ;
        }

        public List<CustomerQuestion> GetCustomerQuestionByUserId(int id)
        {
            var helperCQ = _context.CustomerQuestions.Where(c => c.UserId == id).ToList();
            foreach (var question in helperCQ)
            {
                question.User = _userService.GetUserById(question.UserId);
            }
            return helperCQ;
        }

        public List<CustomerQuestion> GetCustomerQuestionByUsername(string username)
        {
            var helperCQ = _context.CustomerQuestions.Where(c => c.Username == username).ToList();
            foreach (var question in helperCQ)
            {
                question.User = _userService.GetUserByUsername(question.Username);
            }
            return helperCQ;
        }

        public bool CreateCustomerQuestionAnswer(int id, CustomerQuestion question)
        {
            var helper = GetCustomerQuestionById(id);
            if (helper == null || helper.Closed)
                return false;

            helper.Answer = question.Answer;
            helper.Closed = true;
            helper.AnswerDate = DateTime.Now;

            _context.CustomerQuestions.Update(helper);
            _context.SaveChanges();

            var user = _userService.GetUserById(helper.UserId);
            if (user != null)
            {
                _emailService.SendEmail(user.Email, "Vaše pitanje je odgovoreno!", $"Poštovani {user.Name},\n\nVaše pitanje:\n\n{helper.Text}\n\nOdgovor: {helper.Answer}");
            }

            return true;
        }

        public int GetUnansweredQuestionCount()
        {
            return _context.CustomerQuestions.Count(c => !c.Closed);
        }

    }


}
