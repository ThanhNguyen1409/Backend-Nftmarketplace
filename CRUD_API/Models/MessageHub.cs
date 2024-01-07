using Microsoft.AspNetCore.SignalR;
namespace CRUD_API.Models
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            // Gửi thông báo đến tất cả các clients đang kết nối
            await Clients.All.SendAsync(message);
        }
    }
}
