using System.ComponentModel.DataAnnotations;

namespace NFTMarketPlace_Backend.DTO
{
    public class CollectionDTO
    {
        [Required]
        [MaxLength(100)]
        public string CollectionName { get; set; }

        public string CollectionAddress { get; set; }

        public string CollectionSymbol { get; set; }
        public IFormFile CollectionImage { get; set; }
        public string AccountAddress { get; set; }
    }
}
