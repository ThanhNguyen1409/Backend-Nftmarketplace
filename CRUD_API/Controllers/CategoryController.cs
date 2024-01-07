using CRUD_API.Data;
using CRUD_API.DTO;
using CRUD_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CRUD_API.Controllers
{

    [ApiController]
    [Route("/api/category")]
    public class CategoryController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;


        public CategoryController(DbContextCRUD dbContextCRUD, IOptions<AppSetting> appSettings)
        {
            _dbContextCRUD = dbContextCRUD;

        }


        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            var categoryList = await _dbContextCRUD.Categories.ToListAsync();
            if (categoryList == null)
            {
                return NotFound();
            }

            return Ok(categoryList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO model)
        {
            if (_dbContextCRUD.Categories.Any(p => p.categoryName == model.categoryName))
            {
                return BadRequest($"Category {model.categoryName} already exists.");
            }
            var category = new Category
            {
                categoryName = model.categoryName,



            };


            _dbContextCRUD.Categories.Add(category);
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("/api/category/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _dbContextCRUD.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _dbContextCRUD.Categories.Remove(category);
            await _dbContextCRUD.SaveChangesAsync();

            var categoryList = await _dbContextCRUD.Categories
             .ToListAsync();

            return Ok(categoryList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _dbContextCRUD.Categories
        .Where(od => od.categoryId == id)
        .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound(); // Trả về mã lỗi 404 nếu sản phẩm không tồn tại
            }

            return Ok(category);
        }

        [HttpPut("/api/category/update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO model)
        {
            var category = await _dbContextCRUD.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }


            category.categoryName = model.categoryName;

            _dbContextCRUD.Entry(category).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(category);
        }


    }
}
