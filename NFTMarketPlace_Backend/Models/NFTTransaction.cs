using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.Models
{
    [Table("NFTTransaction")]

    public class NFTTransaction
    {
        public int NftTransactionId { get; set; }

        //[Required]
        //public int accountId { get; set; }

        //[ForeignKey("accountId")]
        //public Account Account { get; set; }
        [Required]
        public int tokenId { get; set; }
        [Required]
        public string NftAddress { get; set; }

        [Required]
        public string Event { get; set; }

        [Required]
        [MaxLength(50)]
        public string Price { get; set; }

        [Required]

        public string From { get; set; }
        [Required]

        public string To { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }
    }
}
