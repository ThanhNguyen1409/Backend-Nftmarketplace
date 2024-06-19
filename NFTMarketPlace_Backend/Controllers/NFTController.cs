using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFTMarketPlace_Backend.Data;
using NFTMarketPlace_Backend.DTO;

namespace NFTMarketPlace_Backend.Controllers
{
    [ApiController]
    [Route("/api/nft")]
    public class NFTController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;



        public NFTController(DbContextCRUD dbContextCRUD)
        {
            _dbContextCRUD = dbContextCRUD;

        }

        [HttpGet("listed")]
        public async Task<IActionResult> GetListedNft()
        {
            var nftList = await _dbContextCRUD.nFTs.Where(a => a.IsListed == true).ToListAsync();
            if (nftList.Count == 0)
            {
                return Ok(new object[] { });
            }
            return Ok(nftList);
        }

        [HttpGet("account/{address}")]
        public async Task<IActionResult> GetNftWithAccount(string address, [FromQuery] Boolean listed)
        {

            List<NFT> nftList = await _dbContextCRUD.nFTs.Where(a => a.IsListed == listed && a.Owner == address).ToListAsync();


            if (nftList.Count == 0)
            {
                return Ok(new object[] { });
            }
            return Ok(nftList);
        }
        [HttpPost]
        public async Task<IActionResult> ResellOrTransfer([FromBody] NFTDTO model, [FromQuery] string contract, [FromQuery] int tokenId)
        {
            var nft = await _dbContextCRUD.nFTs
                .Where(a => a.TokenAddress == contract && a.TokenId == tokenId)
                .FirstOrDefaultAsync();
            if (nft == null)
            {
                return Ok(null);
            }
            nft.IsListed = model.IsListed;
            nft.Owner = model.Owner;
            nft.Price = model.Price;

            await _dbContextCRUD.SaveChangesAsync();
            return Ok(nft);

        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNFT([FromBody] NFTDTO model)
        {
            var nft = new NFT
            {
                TokenAddress = model.TokenAddress,
                CollectionId = model.CollectionId,
                Creator = model.Creator,
                IsListed = true,
                Owner = model.Owner,
                Price = model.Price,
                TokenId = model.TokenId,
                TokenURI = model.TokenURI,

            };
            _dbContextCRUD.nFTs.Add(nft);
            await _dbContextCRUD.SaveChangesAsync();
            return Ok(nft);
        }

    }
}
