using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Customer")]
    public class Customer
    {
        
        public int customerId {  get; set; }
        [Required]
        [MaxLength(50)]
        public string customerName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string customerEmail { get; set; }
        [Required]
        [MaxLength(50)]
        public string customerPhone { get; set; }
        [Required]
        [MaxLength(200)]
        public string customerPassword { get; set; }


        public Customer(int customerId, string customerName, string customerEmail, string customerPhone)
        {
            this.customerId = customerId;
            this.customerName = customerName;
            this.customerEmail = customerEmail;
            this.customerPhone = customerPhone;
            
        }

        public Customer() { }
    }
}
