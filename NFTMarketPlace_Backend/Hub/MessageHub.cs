using Microsoft.AspNetCore.SignalR;
using NFTMarketPlace_Backend.Models;

namespace NFTMarketPlace_Backend.Hub
{
    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task SendMessage(Notification notification, NFTTransaction data)
        {

            await Clients.All.SendMessage(notification, data);
        }
    }
}
