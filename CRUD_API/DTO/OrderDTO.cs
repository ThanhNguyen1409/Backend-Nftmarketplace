using System.ComponentModel.DataAnnotations;

namespace CRUD_API.DTO
{
    public class OrderDTO
    {

        [Required]
        public int accountId { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string province { get; set; }
        [Required]
        public string district { get; set; }
        [Required]
        public string ward { get; set; }

        public string orderName { get; set; }
        public string orderEmail { get; set; }
        public string orderPhone { get; set; }
        public decimal orderTotal { get; set; }
        public string orderStatus { get; set; }
        public Boolean isRating { get; set; }
    }
}
