using WebShopSportskeOpreme.Models;

namespace WebShopSportskeOpreme.Interfaces
{
    public interface ICustomerQuestionService
    {
        List<CustomerQuestion> GetAllCustomerQuestions();
        List<CustomerQuestion> GetCustomerQuestionByUserId(int id);
        List<CustomerQuestion> GetCustomerQuestionByUsername(string username);
        CustomerQuestion GetCustomerQuestionById(int id);
        CustomerQuestion CreateCustomerQuestion(CustomerQuestion customerQuestion);
        bool DeleteCustomerQuestion(int id);
        bool CreateCustomerQuestionAnswer(int id, CustomerQuestion customerQuestion);
        int GetUnansweredQuestionCount();
    }
}
