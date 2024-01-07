using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace CRUD_API.Models
{
    [Table("Account")]
    public class Account
    {

        public int accountId { get; set; }
        [Required]
        [MaxLength(50)]
        public string accountName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string accountEmail { get; set; }
        [Required]
        [MaxLength(50)]
        public string accountPhone { get; set; }
        [Required]
        [MaxLength(200)]
        public string accountPassword { get; set; }

        [MaxLength(500)]
        public string RefreshToken { get; set; }

        
        public DateTime RefreshTokenExpiryTime { get; set; }
        public Account(int accountId, string accountName, string accountEmail, string accountPhone)
        {
            this.accountId = accountId;
            this.accountName = accountName;
            this.accountEmail = accountEmail;
            this.accountPhone = accountPhone;

        }

        public Account() { }
    }
}
