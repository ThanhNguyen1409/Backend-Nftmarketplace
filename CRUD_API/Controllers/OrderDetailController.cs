using CRUD_API.Data;
using CRUD_API.DTO;
using CRUD_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CRUD_API.Controllers
{
    [ApiController]
    [Route("/api/orderDetails")]
    public class OrderDetailController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;


        public OrderDetailController(DbContextCRUD dbContextCRUD, IOptions<AppSetting> appSettings)
        {
            _dbContextCRUD = dbContextCRUD;

        }

        [HttpGet]

        public async Task<IActionResult> GetOrderDetailWithId(int orderId)
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
    })
    .ToListAsync();


            return Ok(orderDetails);


        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailDTO model)
        {

            var orderDetail = new OrderDetail
            {
                orderId = model.orderId,
                productId = model.productId,
                quantity = model.quantity
                

            };


            _dbContextCRUD.OrderDetails.Add(orderDetail);
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(orderDetail);
        }



    }
}
