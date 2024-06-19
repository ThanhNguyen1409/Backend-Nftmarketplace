using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFTMarketPlace_Backend.Models
{
    [Table("Collection")]
    public class Collection
    {
        public int CollectionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CollectionName { get; set; }

        [Required]
        public string CollectionAddress { get; set; }
        [Required]
        public string CollectionSymbol { get; set; }
        [Required]

        public string CollectionImage { get; set; }

        [Required]

        public string AccountAddress { get; set; }
        [ForeignKey("AccountAddress")]
        public Account Account { get; set; }
    }
}
