using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFTMarketPlace_Backend.Data;
using NFTMarketPlace_Backend.DTO;
using NFTMarketPlace_Backend.Models;

namespace NFTMarketPlace_Backend.Controllers
{
    [ApiController]
    [Route("/api/offer")]
    public class OfferController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;


        public OfferController(DbContextCRUD dbContextCRUD)
        {
            _dbContextCRUD = dbContextCRUD;

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOffersByNftId(int id)
        {
            var offers = await _dbContextCRUD.offers
                .Where(o => o.NftId == id)
                .ToListAsync();
            if (offers.Count == 0)
            {
                return Ok(new object[] { });
            }

            return Ok(offers);


        }

        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] OfferDTO model)
        {

            var offer = new Offer
            {
                NftId = model.NftId,
                Bidder = model.Bidder,
                Amount = model.Amount,
                Duration = DateTimeOffset.FromUnixTimeMilliseconds(model.Duration).ToOffset(TimeSpan.FromHours(7)).DateTime,

            };


            _dbContextCRUD.offers.Add(offer);
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(offer);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> ClearOffer(int id)
        {
            var offers = await _dbContextCRUD.offers.Where(o => o.NftId == id).ExecuteDeleteAsync();

            return Ok();
        }

    }
}
