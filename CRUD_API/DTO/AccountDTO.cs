using System.ComponentModel.DataAnnotations;

namespace CRUD_API.DTO
{
    public class AccountDTO
    {
        public string accountName { get; set; }
       
        public string accountEmail { get; set; }
        
        public string accountPhone { get; set; }
        
        public string accountPassword { get; set; }

        public int roleId { get; set; }
    }
}
