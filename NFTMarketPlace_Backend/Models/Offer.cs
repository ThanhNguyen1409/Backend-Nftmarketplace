using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.Models
{
    [Table("Offer")]
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int NftId { get; set; }
        [ForeignKey("NftId")]
        public NFT NFT { get; set; }
        [Required]
        public string Bidder { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]

        [Column(TypeName = "DateTime2")]
        public DateTime Duration { get; set; }


    }
}
