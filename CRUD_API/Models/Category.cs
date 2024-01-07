using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_API.Models
{
    [Table("Category")]
    public class Category
    {
        public int categoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string categoryName { get; set; }
       
    }
}
