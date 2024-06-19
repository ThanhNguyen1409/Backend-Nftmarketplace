using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NFTMarketPlace_Backend.Data;
using NFTMarketPlace_Backend.DTO;
using Account = NFTMarketPlace_Backend.Models.Account;

namespace NFTMarketPlace_Backend.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AccountController(DbContextCRUD dbContextCRUD,
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _dbContextCRUD = dbContextCRUD;

            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        //[httpget]
        //public async Task<IActionResult> getallcustomer()
        //{
        //    var accountlist = await _dbcontextcrud.accounts.tolistasync();
        //    if (accountlist == null)
        //        return notfound();
        //    else
        //        return ok(accountlist);
        //}

        //        [HttpPost("register")]
        //        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO model)
        //        {
        //            if (_dbContextCRUD.Accounts.Any(p => p.accountEmail == model.accountEmail))
        //            {
        //                return BadRequest($"Tài khoản đã tồn tại");
        //            }

        //            var account = new Account
        //            {
        //                accountName = model.accountName,
        //                accountEmail = model.accountEmail,
        //                accountPhone = model.accountPhone,
        //                accountPassword = BCrypt.Net.BCrypt.HashPassword(model.accountPassword),

        //            };


        //            _dbContextCRUD.Accounts.Add(account);
        //            await _dbContextCRUD.SaveChangesAsync();
        //            var result = await SaveRole(account.accountId, 2);
        //            return Ok(new { result });
        //        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO model)
        {
            if (_dbContextCRUD.Accounts.Any(p => p.AccountAddress == model.AccountAddress))
            {
                return BadRequest($"Tài khoản đã tồn tại");
            }

            var account = new Account
            {
                AccountName = model.AccountName,
                AccountEmail = model.AccountEmail,
                AccountAddress = model.AccountAddress,
                Avatar = "",
                BannerImage = "",
                BannerVideo = "",
            };


            _dbContextCRUD.Accounts.Add(account);
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(account);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getAccount(string id)
        {
            var account = await _dbContextCRUD.Accounts.FindAsync(id);
            if (account == null)
            {
                return Ok("Account not found");
            }
            return Ok(account);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromForm] AccountDTO model)
        {
            var account = await _dbContextCRUD.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound("Không tìm thấy");
            }

            string avatarUrl = account.Avatar;
            string bannerImage = account.BannerImage;
            string bannerVideo = account.BannerVideo;

            if (model.Avatar != null)
            {
                avatarUrl = await UploadImageToPinataAsync(model.Avatar);
            }

            if (model.BannerImage != null)
            {
                bannerImage = await UploadImageToPinataAsync(model.BannerImage);
            }
            if (model.BannerVideo != null)
            {
                bannerVideo = await UploadImageToPinataAsync(model.BannerVideo);
            }
            account.AccountEmail = model.AccountEmail;
            account.AccountName = model.AccountName;
            account.Avatar = avatarUrl;
            account.BannerImage = bannerImage;
            account.BannerVideo = bannerVideo;




            _dbContextCRUD.Entry(account).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();
            return Ok(account);
        }
        private async Task<string> UploadImageToPinataAsync(IFormFile imageFile)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("pinata_api_key", "77bbf4d6015623aa9597");
                client.DefaultRequestHeaders.Add("pinata_secret_api_key", "6f3ddfb8a08bce570390f486f2de1a4505461bd3732ee2e541c98103eced3cb0");

                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(imageFile.OpenReadStream()), "file", imageFile.FileName);

                var response = await client.PostAsync("https://api.pinata.cloud/pinning/pinFileToIPFS", formData);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);
                var ipfsHash = jsonResponse["IpfsHash"].ToString();

                return $"https://gateway.pinata.cloud/ipfs/{ipfsHash}";
            }
            catch (HttpRequestException ex)
            {
                // Xử lý ngoại lệ khi có lỗi trong quá trình gửi yêu cầu đến API Pinata
                // Trong trường hợp này, bạn có thể xử lý lỗi hoặc throw nó để bên gọi có thể xử lý
                throw new Exception($"Error uploading image to IPFS: {ex.Message}");
            }
        }
    }
}


//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetCustormerWithId(int id)
//        {
//            var customer = await _dbContextCRUD.Customers.Where(c => c.customerId == id).FirstOrDefaultAsync();

//            if (customer == null)
//            {
//                return NotFound();
//            }

//            var response = new
//            {
//                customer.customerEmail,
//                customer.customerName,
//                customer.customerPhone,
//                customer.customerId
//            };

//            return Ok(response);
//        }


//        [HttpPost]
//        [Route("refresh-token")]
//        public async Task<IActionResult> RefreshToken(TokenDTO tokenModel, string email)
//        {
//            if (tokenModel is null)
//            {
//                return BadRequest("Invalid client request");
//            }

//            string accessToken = tokenModel.AccessToken;
//            string refreshToken = tokenModel.RefreshToken;

//            //var principal = GetPrincipalFromExpiredToken(accessToken);
//            //if (principal == null)
//            //{
//            //    return BadRequest("Invalid access token or refresh token");
//            //}

//            //var email = principal.FindFirst(ClaimTypes.Email)?.Value;



//            var account = await _dbContextCRUD.Accounts
//       .FirstOrDefaultAsync(c => c.accountEmail == email);

//            if (account == null || account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.Now)
//            {
//                return BadRequest("Invalid access token or refresh token");
//            }

//            var roleAccount = await _dbContextCRUD.AccountRoles
//               .Where(r => r.accountId == account.accountId)
//               .Include(r => r.Role)
//               .Select(r => new
//               {
//                   r.accountId,
//                   r.roleId,
//                   r.Role.roleName
//               }).FirstOrDefaultAsync();

//            var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, account.accountName),
//                    new Claim(ClaimTypes.Email, account.accountEmail),
//                    new Claim(ClaimTypes.Role, roleAccount.roleName),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };
//            var newAccessToken = GetToken(authClaims);
//            var newRefreshToken = GenerateRefreshToken();

//            account.RefreshToken = newRefreshToken;
//            _dbContextCRUD.Entry(account).State = EntityState.Modified;
//            await _dbContextCRUD.SaveChangesAsync();

//            return new ObjectResult(new
//            {
//                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
//                refreshToken = newRefreshToken,
//                expiration = newAccessToken.ValidTo.AddHours(7),
//            });
//        }

//        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
//        {
//            var tokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateAudience = false,
//                ValidateIssuer = false,
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
//                ValidateLifetime = false
//            };

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
//            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
//                throw new SecurityTokenException("Invalid token");

//            return principal;

//        }


//        //Hàm tạo token
//        private JwtSecurityToken GetToken(List<Claim> authClaims)
//        {
//            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

//            var token = new JwtSecurityToken(
//                issuer: _configuration["JWT:ValidIssuer"],
//                audience: _configuration["JWT:ValidAudience"],
//                expires: DateTime.Now.AddMinutes(1),
//                claims: authClaims,
//                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

//                );

//            return token;
//        }

//        //Hàm tạo refresh token
//        private static string GenerateRefreshToken()
//        {
//            var randomNumber = new byte[64];
//            using var rng = RandomNumberGenerator.Create();
//            rng.GetBytes(randomNumber);
//            return Convert.ToBase64String(randomNumber);
//        }

//        //Hàm lưu AccoutId và RoleId vào table AccountRole
//        private async Task<AccountRoles> SaveRole(int accountId, int roleId)
//        {
//            var result = new AccountRoles
//            {
//                accountId = accountId,
//                roleId = roleId,
//            };

//            _dbContextCRUD.AccountRoles.Add(result);
//            await _dbContextCRUD.SaveChangesAsync();

//            return result;

//        }
//        [HttpPost("CreateRole")]
//        public async Task<IActionResult> CreateRole([FromBody] RoleDTO model)
//        {
//            if (_dbContextCRUD.Roles.Any(p => p.roleName == model.roleName))
//            {
//                return BadRequest($"Role {model.roleName} already exists.");
//            }
//            var role = new Role
//            {
//                roleName = model.roleName,



//            };


//            _dbContextCRUD.Roles.Add(role);
//            await _dbContextCRUD.SaveChangesAsync();

//            return Ok(role);
//        }
//    }
//}
