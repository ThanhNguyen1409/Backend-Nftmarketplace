using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Admin")]
    public class Admin
    {
        public int adminId { get; set; }
        [Required]
        public string adminName { get; set; }
        [Required]
        [EmailAddress]
        public string adminEmail { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất 8 ký tự, bao gồm ít nhất một chữ cái, một số, và một ký tự đặc biệt.")]
        public string adminPassword { get; set; }
    }


}
