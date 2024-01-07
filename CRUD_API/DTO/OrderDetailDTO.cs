using CRUD_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUD_API.DTO
{
    public class OrderDetailDTO
    {
        
        public int orderId { get; set; }
        
       

        public int productId { get; set; }

        public int quantity { get; set; }

        
        public string productName { get; set; }
        public decimal productPrice { get; set; }
        public string Image { get; set; }
    }
}
