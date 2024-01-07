using System.ComponentModel.DataAnnotations;

namespace CRUD_API.DTO
{
    public class CategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string categoryName { get; set; }
    }
}
