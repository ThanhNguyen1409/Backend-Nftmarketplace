using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int notificationId { get; set; }


        [Required]
        public string notificationText { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime notificationTime { get; set; }

        [Required]
        public bool isRead { get; set; }
    }
}
