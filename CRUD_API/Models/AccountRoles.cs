using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("AccountRoles")]
    public class AccountRoles
    {
        public int Id { get; set; }
        public int accountId { get; set; }

        [ForeignKey("accountId")]
        public Account Account { get; set; }

        public int roleId { get; set; }

        [ForeignKey("roleId")]
        public Role Role { get; set; }
    }
}
