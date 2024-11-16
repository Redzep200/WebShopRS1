using Microsoft.AspNetCore.SignalR;

namespace WebShopSportskeOpreme.Hubs
{
    public class CustomerSupportHub : Hub
    {
        public async Task SendMessage(int questionId, string text, int userId, bool closed)
        {
            await Clients.All.SendAsync("ReceiveNewMessage", new { Id = questionId, Text = text, UserId = userId, Closed = closed });
        }

        public async Task NotifyQuestionDeleted(int questionId)
        {
            await Clients.All.SendAsync("QuestionDeleted", questionId);
        }
    }
}