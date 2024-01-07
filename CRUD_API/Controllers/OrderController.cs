using CRUD_API.Data;
using CRUD_API.DTO;
using CRUD_API.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Globalization;
namespace CRUD_API.Controllers
{

    [ApiController]
    [Route("/api/orders")]
    public class OrderController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;
        private readonly AppSetting _appSetting;
        private readonly IHubContext<MessageHub> _hubContext;
        public OrderController(DbContextCRUD dbContextCRUD, IOptions<AppSetting> appSettings, IHubContext<MessageHub> hubContext)
        {
            _dbContextCRUD = dbContextCRUD;
            _appSetting = appSettings.Value;
            _hubContext = hubContext;
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetAllOrderWithAccountId(int id)
        {
            var orderList = await _dbContextCRUD.Orders.Where(od => od.accountId == id).ToListAsync();
            List<List<OrderDetailDTO>> imageOrders = new List<List<OrderDetailDTO>>();

            foreach (var order in orderList)
            {
                var orderDetails = await _dbContextCRUD.OrderDetails
                    .Where(od => od.orderId == order.orderId) // Assuming your OrderDetails model has a property named OrderId
                    .Include(od => od.Product)
                    .Select(od => new OrderDetailDTO
                    {
                        // Map relevant properties from OrderDetails and Product to your DTO
                        productName = od.Product.productName,
                        Image = od.Product.Image,
                        productId = od.Product.productId,
                        productPrice = od.Product.productPrice,
                        quantity = od.quantity,
                        orderId = od.orderId
                        // Add more properties as needed
                    })
                    .ToListAsync();

                imageOrders.Add(orderDetails);
            }
            if (orderList == null)
            {
                return NotFound();
            }

            var mergedOrders = orderList.Select((order, index) => new
            {
                Order = order,
                OrderDetails = imageOrders[index]
            }).ToList();

            return Ok(mergedOrders);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var orderList = await _dbContextCRUD.Orders.ToListAsync();


            if (orderList == null)
            {
                return NotFound();
            }



            return Ok(orderList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO model)
        {

            var order = new Order
            {
                accountId = model.accountId,
                orderTotal = model.orderTotal,
                address = model.address,
                orderDate = DateTime.Now,

                province = model.province, // Assuming ImageUrls is a property in your Product model to store multiple image URLs
                district = model.district,
                ward = model.ward,
                orderStatus = "Đang xử lý"

            };


            _dbContextCRUD.Orders.Add(order);

            await _dbContextCRUD.SaveChangesAsync();





            return Ok(order);
        }

        [HttpPut("/api/orders/update/status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] string status, int id)
        {
            var order = await _dbContextCRUD.Orders.FindAsync(id);
            order.orderStatus = status;
            _dbContextCRUD.Entry(order).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(order);
        }
        [HttpPost("/api/orders/send_email")]
        public async Task<IActionResult> SendEmailConfirmation([FromBody] OrderDTO model, [FromQuery] int orderId)
        {
            try
            {
                var order = new OrderDTO
                {

                    accountId = model.accountId,
                    orderTotal = model.orderTotal,
                    address = model.address,


                    province = model.province, // Assuming ImageUrls is a property in your Product model to store multiple image URLs
                    district = model.district,
                    ward = model.ward,
                    orderEmail = model.orderEmail,
                    orderName = model.orderName,
                    orderPhone = model.orderPhone,

                };
                await SendOrderConfirmationEmail(order, orderId);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }

        private async Task SendOrderConfirmationEmail(OrderDTO order, int orderId)
        {
            try
            {
                var orderDetails = await _dbContextCRUD.OrderDetails
                    .Where(od => od.orderId == orderId)
                    .Include(od => od.Product)
                    .Select(od => new OrderDetailDTO
                    {

                        orderId = od.orderId,
                        productId = od.productId,
                        productName = od.Product.productName,
                        productPrice = od.Product.productPrice,
                        Image = od.Product.Image,
                        quantity = od.quantity

                    }).ToListAsync();



                var custormer = await _dbContextCRUD.Accounts
                    .Where(c => c.accountId == order.accountId)
                    .Select(c => new AccountDTO
                    {

                        accountName = c.accountName,
                        accountPhone = c.accountPhone,
                        accountEmail = c.accountEmail

                    }).FirstOrDefaultAsync();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Store", "ptclone21@gmail.com"));
                message.To.Add(new MailboxAddress($"{order.orderName}", $"{order.orderEmail}"));
                message.Subject = "Order Confirmation";

                // Add HTML content with product information
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = $"<p>Thank you for your order. Order ID: {orderId}</p>";

                bodyBuilder.HtmlBody = GetProductHtmlContent(order, orderDetails, custormer, orderId);

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("ptclone21@gmail.com", "htly wgwo secg ielm");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private string GetProductHtmlContent(OrderDTO order, List<OrderDetailDTO> orderDetails, AccountDTO customer, int orderId)
        {
            // Đọc nội dung từ file HTML
            var templatePath = Path.Combine(_appSetting.TemplatePath, "Order2.html");
            var templateHtml = System.IO.File.ReadAllText(templatePath);
            var formatTotal = string.Format(new CultureInfo("vi-VN"), "{0:C}", order.orderTotal);
            // Thay thế các giá trị trong template
            templateHtml = templateHtml.Replace("{order_id}", orderId.ToString());
            templateHtml = templateHtml.Replace("{customer_name}", order.orderName);
            templateHtml = templateHtml.Replace("{order_address}", order.province + ',' + order.district + ',' + order.ward + ',' + order.address);
            templateHtml = templateHtml.Replace("{customer_phone}", order.orderPhone);
            templateHtml = templateHtml.Replace("{customer_email}", order.orderEmail);

            var productHtml = "";

            foreach (var orderDetail in orderDetails)
            {
                var formattedPrice = string.Format(new CultureInfo("vi-VN"), "{0:C}", orderDetail.productPrice);
                var figureHtml = $@"
        <figure class='figure'>
            <img src='{orderDetail.Image}' class='figure-img img-fluid rounded' alt=''>
            <div class='details'>
                <p>{orderDetail.productName}</p>
                <p>{formattedPrice}</p>
                <br />
                <p>Số lượng: {orderDetail.quantity}</p>
            </div>
        </figure>";

                productHtml += figureHtml;
            }

            // Kiểm tra giá trị của productHtml
            Console.WriteLine("Product HTML: ");
            Console.WriteLine(productHtml);

            templateHtml = templateHtml.Replace("{product_content}", productHtml);

            templateHtml = templateHtml.Replace("{order_total}", formatTotal);

            return templateHtml;
        }

        [HttpPost("/api/placeOrder")]
        public async Task<IActionResult> PlaceOrder()
        {
            // Xử lý đặt hàng

            // Gửi thông báo tới tất cả các clients thông qua SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "New order placed!");

            return Ok();
        }
    }

}
