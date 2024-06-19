

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

using NFTMarketPlace_Backend.Data;
using NFTMarketPlace_Backend.DTO;
using NFTMarketPlace_Backend.Models;

namespace nftmarketplace_backend.controllers
{

    [ApiController]
    [Route("/api/collection")]
    public class CollectionController : Controller
    {
        private readonly DbContextCRUD _dbcontextcrud;
        private readonly IHttpClientFactory _clientfactory;


        public CollectionController(DbContextCRUD dbcontextcrud, IHttpClientFactory clientfactory)
        {
            _dbcontextcrud = dbcontextcrud;
            _clientfactory = clientfactory;
        }


        [HttpGet]
        public async Task<IActionResult> getcategory()
        {
            var categorylist = await _dbcontextcrud.Collections.ToListAsync();
            if (categorylist.Count == 0)
            {
                return NotFound();
            }

            return Ok(categorylist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromForm] CollectionDTO model)
        {
            if (_dbcontextcrud.Collections.Any(p => p.CollectionName == model.CollectionName))
            {
                return BadRequest($"Collection with name '{model.CollectionName}' already exists.");
            }

            if (model.CollectionImage != null)
            {
                string imageurl = null;

                try
                {
                    var formdata = new MultipartFormDataContent();
                    formdata.Add(new StreamContent(model.CollectionImage.OpenReadStream()), "file", model.CollectionImage.FileName);

                    var client = _clientfactory.CreateClient();
                    client.DefaultRequestHeaders.Add("pinata_api_key", "77bbf4d6015623aa9597");
                    client.DefaultRequestHeaders.Add("pinata_secret_api_key", "6f3ddfb8a08bce570390f486f2de1a4505461bd3732ee2e541c98103eced3cb0");

                    var response = await client.PostAsync("https://api.pinata.cloud/pinning/pinfiletoipfs", formdata);

                    response.EnsureSuccessStatusCode();

                    var responsecontent = await response.Content.ReadAsStringAsync();
                    var jsonresponse = JObject.Parse(responsecontent);
                    var ipfshash = jsonresponse["IpfsHash"].ToString();

                    imageurl = $"https://gateway.pinata.cloud/ipfs/{ipfshash}";
                }
                catch (HttpRequestException ex)
                {
                    // handle exception here
                    // you can log the error or return an error response
                    return StatusCode(500, "error uploading image to ipfs: " + ex.Message);
                }

                // save the image url in the database
                var collection = new Collection
                {
                    CollectionName = model.CollectionName,
                    CollectionImage = imageurl,
                    AccountAddress = model.AccountAddress,
                    CollectionSymbol = model.CollectionSymbol,
                    CollectionAddress = model.CollectionAddress,

                };

                _dbcontextcrud.Collections.Add(collection);
                await _dbcontextcrud.SaveChangesAsync();

                return Ok(collection);
            }


            return BadRequest("không có tệp hình ảnh được tải lên.");
        }

        [HttpDelete("/api/category/delete/{id}")]
        public async Task<IActionResult> deletecategory(int id)
        {
            var category = await _dbcontextcrud.Collections.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _dbcontextcrud.Collections.Remove(category);
            await _dbcontextcrud.SaveChangesAsync();

            var categorylist = await _dbcontextcrud.Collections
             .ToListAsync();

            return Ok(categorylist);
        }

        [HttpGet("/api/collections/{id}")]
        public async Task<ActionResult> GetListCollectionWithAccount(string id)
        {
            var collection = await _dbcontextcrud.Collections
        .Where(od => od.AccountAddress == id)
        .ToListAsync();

            if (collection.Count == 0)
            {
                return NotFound(); // trả về mã lỗi 404 nếu sản phẩm không tồn tại
            }

            return Ok(collection);
        }

        [HttpGet("{collectionId}")]
        public async Task<ActionResult> GetCollectionWithId(int collectionId)
        {
            var collection = await _dbcontextcrud.Collections
        .Where(od => od.CollectionId == collectionId)
        .FirstOrDefaultAsync();

            if (collection == null)
            {
                return NotFound(); // trả về mã lỗi 404 nếu sản phẩm không tồn tại
            }

            return Ok(collection);
        }

        [HttpGet("collections/nft")]
        public async Task<ActionResult> GetCollectionsByNft([FromQuery] List<int> id)
        {

            if (id == null || !id.Any())
            {
                return BadRequest("Invalid or empty ID list.");
            }

            var collections = await _dbcontextcrud.Collections
                                             .Where(c => id.Contains(c.CollectionId))
                                             .ToListAsync();

            if (!collections.Any())
            {
                return NotFound("No collections found for the given IDs.");
            }

            return Ok(collections);
        }

        //[httpput("/api/category/update/{id}")]
        //public async task<iactionresult> updatecategory(int id, [frombody] categorydto model)
        //{
        //    var category = await _dbcontextcrud.categories.findasync(id);

        //    if (category == null)
        //    {
        //        return notfound();
        //    }
        //    category.categoryname = model.categoryname;

        //    _dbcontextcrud.entry(category).state = entitystate.modified;
        //    await _dbcontextcrud.savechangesasync();

        //    return ok(category);
        //}


    }
}
