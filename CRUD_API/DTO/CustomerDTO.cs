using System.ComponentModel.DataAnnotations;

namespace CRUD_API.DTO
{
    public class CustomerDTO
    {
        public string customerName { get; set; }
        
        [EmailAddress]
        public string customerEmail { get; set; }
        
        public string customerPhone { get; set; }
        
        public string customerPassword { get; set; }
    }
}
