namespace NFTMarketPlace_Backend.DTO
{
    public class NotificationDTO
    {

        public string notificationText { get; set; }
        public DateTime notificationTime { get; set; }

        public bool isRead { get; set; }
    }
}
