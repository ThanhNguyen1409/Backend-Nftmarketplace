using CRUD_API.Data;
using CRUD_API.DTO;
using CRUD_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_API.Controllers
{
    [ApiController]
    [Route("/api/rating")]
    public class RatingController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;

        public RatingController(DbContextCRUD dbContextCRUD)
        {
            _dbContextCRUD = dbContextCRUD;
        }

        [HttpPost]
        public async Task<IActionResult> PostRating([FromBody] List<RatingDTO> models)
        {

            foreach (var model in models)
            {
                var order = await _dbContextCRUD.Orders.FindAsync(model.orderId);
                order.isRating = true;
                _dbContextCRUD.Entry(order).State = EntityState.Modified;
                var rating = new Rating
                {
                    orderId = model.orderId,
                    productId = model.productId,
                    ratingStar = model.ratingStar,
                    ratingDate = DateTime.Now,
                    ratingText = model.ratingText,
                };
                _dbContextCRUD.Ratings.Add(rating);

            }
            await _dbContextCRUD.SaveChangesAsync();
            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingProductId(int id)
        {
            var ratingList = await _dbContextCRUD.Ratings
                .Where(r => r.productId == id)
                .Include(r => r.Order).ThenInclude(o => o.Account)
                .Select(r => new
                {
                    r.ratingText,
                    r.ratingStar,
                    r.ratingDate,
                    accountName = r.Order.Account.accountName

                }
            ).ToListAsync();
            return Ok(ratingList);
        }
    }
}
