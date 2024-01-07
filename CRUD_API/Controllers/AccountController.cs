using CRUD_API.Data;
using CRUD_API.DTO;
using CRUD_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Account = CRUD_API.Models.Account;

namespace CRUD_API.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly DbContextCRUD _dbContextCRUD;

        private readonly IConfiguration _configuration;

        public AccountController(DbContextCRUD dbContextCRUD,
            IConfiguration configuration)
        {
            _dbContextCRUD = dbContextCRUD;

            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            var accountList = await _dbContextCRUD.Accounts.ToListAsync();
            if (accountList == null)
                return NotFound();
            else
                return Ok(accountList);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO model)
        {
            if (_dbContextCRUD.Accounts.Any(p => p.accountEmail == model.accountEmail))
            {
                return BadRequest($"Tài khoản đã tồn tại");
            }

            var account = new Account
            {
                accountName = model.accountName,
                accountEmail = model.accountEmail,
                accountPhone = model.accountPhone,
                accountPassword = BCrypt.Net.BCrypt.HashPassword(model.accountPassword),

            };


            _dbContextCRUD.Accounts.Add(account);
            await _dbContextCRUD.SaveChangesAsync();
            var result = await SaveRole(account.accountId, 2);
            return Ok(new { result });





        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO model)
        {

            var account = await _dbContextCRUD.Accounts
       .FirstOrDefaultAsync(c => c.accountEmail == model.accountEmail);

            if (account == null || !BCrypt.Net.BCrypt.Verify(model.accountPassword, account.accountPassword))
            {
                return BadRequest("Tên người dùng hoặc mật khẩu không hợp lệ");
            }
            var response = new
            {
                accountEmail = account.accountEmail,
                accountName = account.accountName,
                accountPhone = account.accountPhone,
                accountId = account.accountId
            };

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.accountName),
                    new Claim(ClaimTypes.Email, account.accountEmail),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var token = GetToken(authClaims);
            //Tạo refresh token 
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            account.RefreshToken = refreshToken;
            account.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            _dbContextCRUD.Entry(account).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();
            // Xác thực thành công
            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo.AddHours(7),
                refreshToken,
                account = response
            });


        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustormerWithId(int id)
        {
            var customer = await _dbContextCRUD.Customers.Where(c => c.customerId == id).FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            var response = new
            {
                customerEmail = customer.customerEmail,
                customerName = customer.customerName,
                customerPhone = customer.customerPhone,
                customerId = customer.customerId
            };

            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountDTO model)
        {
            var account = await _dbContextCRUD.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound("Không tìm thấy");
            }

            account.accountEmail = model.accountEmail;
            account.accountName = model.accountName;
            account.accountPhone = model.accountPhone;




            if (model.accountPassword != null)
            {
                account.accountPassword = BCrypt.Net.BCrypt.HashPassword(model.accountPassword);
            }
            _dbContextCRUD.Entry(account).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenModel, string email)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            //var principal = GetPrincipalFromExpiredToken(accessToken);
            //if (principal == null)
            //{
            //    return BadRequest("Invalid access token or refresh token");
            //}

            //var email = principal.FindFirst(ClaimTypes.Email)?.Value;


            var account = await _dbContextCRUD.Accounts
       .FirstOrDefaultAsync(c => c.accountEmail == email);

            if (account == null || account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.accountName),
                    new Claim(ClaimTypes.Email, account.accountEmail),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            var newAccessToken = GetToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            _dbContextCRUD.Entry(account).State = EntityState.Modified;
            await _dbContextCRUD.SaveChangesAsync();

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
                expiration = newAccessToken.ValidTo.AddHours(7),
            });
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }


        //Hàm tạo token
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)

                );

            return token;
        }

        //Hàm tạo refresh token
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //Hàm lưu AccoutId và RoleId vào table AccountRole
        private async Task<AccountRoles> SaveRole(int accountId, int roleId)
        {
            var result = new AccountRoles
            {
                accountId = accountId,
                roleId = roleId,
            };

            _dbContextCRUD.AccountRoles.Add(result);
            await _dbContextCRUD.SaveChangesAsync();

            return result;

        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO model)
        {
            if (_dbContextCRUD.Roles.Any(p => p.roleName == model.roleName))
            {
                return BadRequest($"Role {model.roleName} already exists.");
            }
            var role = new Role
            {
                roleName = model.roleName,



            };


            _dbContextCRUD.Roles.Add(role);
            await _dbContextCRUD.SaveChangesAsync();

            return Ok(role);
        }
    }
}
