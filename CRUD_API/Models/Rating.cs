using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        public int ratingId { get; set; }

        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public Order Order { get; set; }

        public int productId { get; set; }
        [ForeignKey("productId")]
        public Product Product { get; set; }


        [MaxLength(200)]
        public string ratingText { get; set; }

        [Range(1, 5, ErrorMessage = "Giá trị phải nằm trong khoảng từ 1 đến 5.")]
        public int ratingStar { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime ratingDate { get; set; }
    }
}
