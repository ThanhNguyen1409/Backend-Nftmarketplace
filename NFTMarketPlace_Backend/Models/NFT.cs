using Newtonsoft.Json;
using NFTMarketPlace_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("NFT")]
public class NFT
{
    [Key]
    public int NftId { get; set; }
    [Required]
    public int TokenId { get; set; }
    [Required]
    public int CollectionId { get; set; }

    [ForeignKey("CollectionId")]
    public Collection Collection { get; set; }

    [Required]
    [MaxLength(500)]
    public string TokenAddress { get; set; }

    [Required]

    public float Price { get; set; }

    [Required]
    public string Owner { get; set; }

    [Required]
    public string Creator { get; set; }


    [Required]
    [MaxLength(500)]
    public string TokenURI { get; set; }

    public Boolean IsListed { get; set; }
}
