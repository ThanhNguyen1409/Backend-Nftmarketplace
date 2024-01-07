using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Role")]
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
    }
}
