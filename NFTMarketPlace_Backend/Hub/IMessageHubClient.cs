using NFTMarketPlace_Backend.Models;

namespace NFTMarketPlace_Backend.Hub
{
    public interface IMessageHubClient
    {
        //Định nghĩa các phương thức và tên của phương thức sẽ là tên của sự kiện Hub
        Task SendMessage(Notification notification, NFTTransaction data);
    }
}
