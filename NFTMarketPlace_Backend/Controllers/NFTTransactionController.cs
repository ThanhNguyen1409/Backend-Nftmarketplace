using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFTMarketPlace_Backend.Data;
using NFTMarketPlace_Backend.DTO;

namespace NFTMarketPlace_Backend.Controllers
{
    [ApiController]
    [Route("/api/transaction")]
    public class NFTTransactionController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;

        public NFTTransactionController(DbContextCRUD dbContextCRUD)
        {
            _dbContextCRUD = dbContextCRUD;

        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] NFTTransactionDTO dTO)
        {
            var transaction = new Models.NFTTransaction
            {
                NftAddress = dTO.NftAddress,
                Event = dTO.Event,
                From = dTO.From,
                To = dTO.To,
                Price = dTO.Price,
                Date = DateTime.Now,
                tokenId = dTO.tokenId,


            };
            await _dbContextCRUD.nFTTransactions.AddAsync(transaction);
            await _dbContextCRUD.SaveChangesAsync();
            return Ok(transaction);
        }
        [HttpGet]
        public async Task<IActionResult> GetTransaction([FromQuery] string address, [FromQuery] int tokenId)
        {
            var transactions = await _dbContextCRUD.nFTTransactions
                .Where(t => t.tokenId == tokenId && t.NftAddress == address)
                .ToListAsync();

            if (transactions.Count == 0)
            {
                return NotFound(null);
            }
            return Ok(transactions);
        }
    }
}
