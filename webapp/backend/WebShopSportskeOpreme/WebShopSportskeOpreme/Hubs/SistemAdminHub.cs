using Microsoft.AspNetCore.SignalR;

namespace WebShopSportskeOpreme.Hubs
{
    public class AdminHub : Hub
    {
        public async Task NotifyRoleChanged(int userId, int newRoleId)
        {
            await Clients.All.SendAsync("UserRoleChanged", userId, newRoleId);
        }
    }
}
