using CRUD_API.Data;
using CRUD_API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace CRUD_API.Controllers
{

    [ApiController]
    [Route("/api/products")]
    public class ProductsController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;
        private readonly AppSetting _appSetting;
        private const string ImgurClientId = "42ec91607c2ae94";
        public ProductsController(DbContextCRUD dbContextCRUD, IOptions<AppSetting> appSettings)
        {
            _dbContextCRUD = dbContextCRUD;
            _appSetting = appSettings.Value;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllProducts()
        {
            var productListWithAverageRating = await _dbContextCRUD.Products
    .GroupJoin(
        _dbContextCRUD.Ratings,
        product => product.productId,
        rating => rating.productId,
        (product, ratings) => new
        {
            product.productId,
            product.categoryId,
            product.Image,
            product.productName,
            product.productPrice,
            product.subImage,
            product.ImageUrls,
            product.productDes,
            product.ImageUrlsJson,
            product.Category.categoryName,

            //RatingStarList = ratings
            //    .Select(r => r.ratingStar)
            //    .ToList(),
            averageRating = ratings.Any() ? Math.Ceiling(ratings.Average(r => r.ratingStar) * 2) / 2 : 0,
        }
    )
    .ToListAsync();




            return Ok(productListWithAverageRating);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO model)
        {
            if (_dbContextCRUD.Products.Any(p => p.productName == model.ProductName))
            {
                return BadRequest($"Product with name '{model.ProductName}' already exists.");
            }

            if (model.Images != null && model.Images.Count > 0)
            {
                List<string> imageUrls = new List<string>();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {ImgurClientId}");

                    foreach (var image in model.Images)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = image.FileName;

                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new StreamContent(image.OpenReadStream())
                                {
                                    Headers =
                            {
                                ContentLength = image.Length,
                                ContentType = new MediaTypeHeaderValue(image.ContentType)
                            }
                                }, "image", fileName);

                                var response = await httpClient.PostAsync("https://api.imgur.com/3/image", content);

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    var jsonResponse = JObject.Parse(responseContent);

                                    if (jsonResponse["success"].Value<bool>())
                                    {
                                        var imageUrl = jsonResponse["data"]["link"].Value<string>();
                                        imageUrls.Add(imageUrl);
                                    }
                                }
                            }
                        }

                    }

                    if (imageUrls.Count >= 2)
                    {
                        // Save the image URLs in the database
                        var product = new Product
                        {
                            productName = model.ProductName,
                            productPrice = model.ProductPrice,
                            productDes = model.ProductDes,
                            ImageUrls = imageUrls, // Assuming ImageUrls is a property in your Product model to store multiple image URLs
                            Image = imageUrls[0],
                            subImage = imageUrls[1],
                            categoryId = model.categoryId

                        };

                        _dbContextCRUD.Products.Add(product);
                        await _dbContextCRUD.SaveChangesAsync();

                        return Ok(product);
                    }
                }
            }

            return BadRequest("Không có tệp hình ảnh được tải lên.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductWithId(int id)
        {
            var product = await _dbContextCRUD.Products
                .Where(p => p.productId == id)
            .GroupJoin(
       _dbContextCRUD.Ratings,
       product => product.productId,
       rating => rating.productId,
       (product, ratings) => new
       {
           product.productId,
           product.categoryId,
           product.Image,
           product.productName,
           product.productPrice,
           product.subImage,
           product.ImageUrls,
           product.productDes,
           product.ImageUrlsJson,
           product.Category.categoryName,

           averageRating = ratings.Any() ? Math.Ceiling(ratings.Average(r => r.ratingStar) * 2) / 2 : 0,
       }
   )
   .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Không tìm thấy sản phầm "); // Trả về mã lỗi 404 nếu sản phẩm không tồn tại
            }

            return Ok(product);
        }

        [HttpPut("/api/products/update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO model)
        {
            var product = await _dbContextCRUD.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            if (model.Images != null && model.Images.Count > 0)
            {
                List<string> imageUrls = new List<string>();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {ImgurClientId}");

                    foreach (var image in model.Images)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = image.FileName;

                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new StreamContent(image.OpenReadStream())
                                {
                                    Headers =
                            {
                                ContentLength = image.Length,
                                ContentType = new MediaTypeHeaderValue(image.ContentType)
                            }
                                }, "image", fileName);

                                var response = await httpClient.PostAsync("https://api.imgur.com/3/image", content);

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    var jsonResponse = JObject.Parse(responseContent);

                                    if (jsonResponse["success"].Value<bool>())
                                    {
                                        var imageUrl = jsonResponse["data"]["link"].Value<string>();
                                        imageUrls.Add(imageUrl);
                                    }
                                }
                            }
                        }

                    }
                    if (imageUrls.Count >= 2)
                    {
                        product.Image = imageUrls[0];
                        product.subImage = imageUrls[1];
                        product.ImageUrls = imageUrls;

                    }
                    else
                    {
                        return BadRequest("Sản phẩm phải có 2 hình ảnh trở lên");
                    }

                }
            }
            product.productName = model.ProductName;
            product.productPrice = model.ProductPrice;
            product.productDes = model.ProductDes;
            product.categoryId = model.categoryId;
            _dbContextCRUD.Entry(product).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("/api/products/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContextCRUD.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContextCRUD.Products.Remove(product);
            await _dbContextCRUD.SaveChangesAsync();

            var productList = await _dbContextCRUD.Products
             .Include(od => od.Category)
             .Where(od => od.categoryId == od.Category.categoryId)
             .Select(od => new
             {
                 od.productId,
                 od.categoryId,
                 od.Image,
                 od.productName,
                 od.productPrice,
                 od.subImage,
                 od.ImageUrls,
                 od.ImageUrlsJson,
                 od.Category.categoryName
             })
             .ToListAsync();

            return Ok(productList);
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetProductWithCategory([FromQuery] List<int> id)
        {
            if (id.Count == 0)
            {
                var productList = await _dbContextCRUD.Products.ToListAsync();
                return Ok(productList);
            }
            else
            {
                var products = await _dbContextCRUD.Products
                    .Where(od => id.Contains(od.categoryId))
                    .Include(od => od.Category)
                    .Select(od => new
                    {
                        od.productId,
                        od.productName,
                        od.productPrice,
                        od.Image,
                        od.subImage,
                        od.ImageUrls,
                        od.ImageUrlsJson,
                        od.Category.categoryId,
                        od.Category.categoryName,
                        od.productDes,
                    })
                    .ToListAsync();

                return Ok(products);
            }
        }

        [HttpGet("/api/products/search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            // Xử lý tìm kiếm và trả về kết quả
            var results = await _dbContextCRUD.Products
                .Where(p => p.productName.Contains(query))
                .ToListAsync();

            return Ok(results);
        }
    }
}
