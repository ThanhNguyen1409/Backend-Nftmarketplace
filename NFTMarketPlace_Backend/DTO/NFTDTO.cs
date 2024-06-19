using System.ComponentModel.DataAnnotations;

namespace NFTMarketPlace_Backend.DTO
{
    public class NFTDTO
    {
        public int TokenId { get; set; }

        public int CollectionId { get; set; }

        public string TokenAddress { get; set; }


        public string Owner { get; set; }

        public string Creator { get; set; }

        [Required]

        public float Price { get; set; }


        public string TokenURI { get; set; }

        public Boolean IsListed { get; set; }


    }
}
