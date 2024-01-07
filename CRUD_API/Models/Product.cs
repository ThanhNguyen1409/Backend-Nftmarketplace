using CRUD_API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Product")]
public class Product
{
    
    public int productId { get; set; }

    public int categoryId { get; set; }

    [ForeignKey("categoryId")]
    public Category Category { get; set; }

    [Required]
    [MaxLength(50)]
    public string productName { get; set; }
    
    [Required]
    [Column(TypeName = "Money")]
    public decimal productPrice { get; set; }

    [Required]
    [MaxLength(500)]
    public string productDes { get; set; }
    // Property to store multiple image URLs as a JSON serialized string

    [Required]
    [MaxLength(250)]
    public string ImageUrlsJson { get; set; }

    [Required]
    [MaxLength(50)]
    public string Image { get; set; }

    [Required]
    [MaxLength(50)]
    public string subImage { get; set; }

    



    // Transient property to deserialize the JSON data
    [NotMapped]
    public List<string> ImageUrls
    {
        get
        {
            if (string.IsNullOrEmpty(ImageUrlsJson))
                return new List<string>();
            return JsonConvert.DeserializeObject<List<string>>(ImageUrlsJson);
        }
        set
        {
            ImageUrlsJson = JsonConvert.SerializeObject(value);
        }
    }
}
