using educoreLMS.Dtos;
using educoreLMS.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace educoreLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return Unauthorized("Invalid credentials");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email ?? "")
    };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new Exception("Jwt:Key missing")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresInMinutes"] ?? "60"));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires,
                authRole = roles.FirstOrDefault(),
                authUserId = user.Id
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var res = await _userManager.CreateAsync(user, dto.Password);
            if (!res.Succeeded) return BadRequest(res.Errors);
            if (!string.IsNullOrEmpty(dto.Role)) await _userManager.AddToRoleAsync(user, dto.Role);
            return Ok();
        }
    }
}
