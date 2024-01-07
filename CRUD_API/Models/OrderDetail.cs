using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int orderDetailId { get; set; }
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public Order Order { get; set; }
        
        public int productId { get; set; }

        [ForeignKey("productId")]
        public Product Product { get; set; }
        [Required]
        public int quantity { get; set; }

        public OrderDetail() { }
        public OrderDetail(int orderDetailId, int orderId, int productId, int quantity)
        {
            this.orderDetailId = orderDetailId;
            this.orderId = orderId;
            this.productId = productId;
            this.quantity = quantity;
        }
    }
}
