using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Order")]
    public class Order
    {
        public int orderId { get; set; }
        [Required]


        public int accountId { get; set; }

        [ForeignKey("accountId")]
        public Account Account { get; set; }

        [Required]
        [MaxLength(50)]
        public string address { get; set; }

        [Required]
        [MaxLength(50)]
        public string province { get; set; }
        [Required]
        [MaxLength(50)]
        public string district { get; set; }
        [Required]
        [MaxLength(50)]
        public string ward { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "DateTime2")]
        public DateTime orderDate { get; set; }

        [Column(TypeName = "Money")]
        public decimal orderTotal { get; set; }

        [Required]
        [MaxLength(50)]
        public string orderStatus { get; set; }


        public Boolean isRating { get; set; }
    }
}
