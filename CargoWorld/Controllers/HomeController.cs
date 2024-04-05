using Core.Models;
using Data;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using Service.Interfaces;
using Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml;

namespace CargoWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        private AppDbContext db = new AppDbContext();

		public HomeController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
		public ActionResult Index()
        {
            
            return View("Index");
        }
		[HttpGet]
		public ActionResult About()
        {
            return View("About");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        /// <summary>
        ///     Аутентифицирует пользователя
        /// </summary>
        /// <param name="model">DTO для аутетентификации</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromForm] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var res = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
                

                await _userManager.UpdateAsync(user);

                this.HttpContext.Response.Cookies.Append("token", new JwtSecurityTokenHandler().WriteToken(token));

                return RedirectToAction("Index", "Cargo");
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Регистрирует пользователя
        /// </summary>
        /// <param name="model">DTO для регистрации</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Register([FromForm] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var id = Guid.NewGuid().ToString();
            AppUser appUser = new()
            {
                Email = model.Email,
                SecurityStamp = id,
                UserName = model.Email,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            var userRoles = await _userManager.GetRolesAsync(appUser);

            

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, model.Email),
                new(ClaimTypes.NameIdentifier, id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = CreateToken(authClaims);

            if (!result.Succeeded)
            {
                this.HttpContext.Response.Cookies.Append("token", new JwtSecurityTokenHandler().WriteToken(token));
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Регистрирует администратора
        /// </summary>
        /// <param name="model">DTO для регистрации</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterAdmin([FromForm] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            AppUser appUser = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(appUser, UserRoles.User);
            }

            return Ok();
        }

        /// <summary>
        ///     Обновляет JWT токен
        /// </summary>
        /// <param name="refreshTokenModel">DTO для токена</param>
        /// <returns></returns>
        //    [HttpPost]
        //    [Route("refresh-token")]
        //    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(RefreshTokenModel refreshTokenModel)
        //    {
        //        if (refreshTokenModel is null)
        //        {
        //            return BadRequest("Invalid client request");
        //        }

        //        var accessToken = refreshTokenModel.AccessToken;
        //        var refreshToken = refreshTokenModel.RefreshToken;

        //        var principal = GetPrincipalFromExpiredToken(accessToken);

        //        if (principal == null)
        //        {
        //            return BadRequest("Invalid access token or refresh token");
        //        }

        //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        //#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //        var username = principal.Identity.Name;
        //#pragma warning restore CS8602 // Dereference of a possibly null reference.
        //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        //        var user = await _userManager.FindByNameAsync(username);

        //        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        //        {
        //            return BadRequest("Invalid access token or refresh token");
        //        }

        //        var newAccessToken = CreateToken(principal.Claims.ToList());
        //        var newRefreshToken = GenerateRefreshToken();

        //        user.RefreshToken = newRefreshToken;
        //        await _userManager.UpdateAsync(user);

        //        return Ok(new RefreshTokenResponse
        //        {
        //            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
        //            RefreshToken = newRefreshToken,
        //            Expiration = newAccessToken.ValidTo,
        //        });
        //    }

        /// <summary>
        ///     Отзывает токен у пользователя
        /// </summary>
        /// <param name="username">Имя пользователя, у которого надо отозвать токен</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<ActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("Invalid user name");
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        /// <summary>
        ///     Отзывает все токены
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<ActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }


        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);

            var token = new JwtSecurityToken(_configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }


        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
