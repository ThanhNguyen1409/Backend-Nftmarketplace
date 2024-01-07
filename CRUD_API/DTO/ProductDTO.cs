using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace CRUD_API.DTO
{
    public class ProductDTO
    {



        
        public string ProductName { get; set; }

        
        public int categoryId { get; set; }
        
        public decimal ProductPrice { get; set; }

        
        public string ProductDes { get; set; }

        
        public List<IFormFile> Images { get; set; }


    }
}
