//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using NFTMarketPlace_Backend.Data;

//namespace NFTMarketPlace_Backend.Controllers
//{
//    [ApiController]
//    [Route("/api/notifications")]
//    public class NotificationController : Controller
//    {
//        private readonly DbContextCRUD _dbContextCRUD;

//        public NotificationController(DbContextCRUD dbContextCRUD)
//        {
//            _dbContextCRUD = dbContextCRUD;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllNotification()
//        {
//            var notificationList = await _dbContextCRUD.Notifications.OrderByDescending(n => n.notificationTime).ToListAsync();
//            return Ok(notificationList);
//        }

//        [HttpPut("/api/notifications/update")]
//        public async Task<IActionResult> UpdateNotification()
//        {
//            var notifications = await _dbContextCRUD.Notifications.Where(n => n.isRead == false).ToListAsync();
//            foreach (var notification in notifications)
//            {
//                notification.isRead = true;
//            }
//            _dbContextCRUD.Notifications.UpdateRange(notifications);
//            await _dbContextCRUD.SaveChangesAsync();
//            return Ok(notifications);
//        }
//    }
//}
