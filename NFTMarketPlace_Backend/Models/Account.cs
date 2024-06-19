using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public string AccountAddress { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string AccountEmail { get; set; }

        public string Avatar { get; set; }

        public string BannerImage { get; set; }

        public string BannerVideo { get; set; }
        //[MaxLength(500)]
        //public string RefreshToken { get; set; }


        //public DateTime RefreshTokenExpiryTime { get; set; }
        //public Account(int accountId, string accountName, string accountEmail, string accountPhone)
        //{
        //    this.accountId = accountId;
        //    this.accountName = accountName;
        //    this.accountEmail = accountEmail;
        //    this.accountPhone = accountPhone;

        //}

        //public Account() { }
    }
}
