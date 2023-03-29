using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext appDbContext;
        public AuthController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            this.appDbContext = appDbContext;
        }


        public static User user = new User();
        public static Employee employee = new Employee();

        [HttpPost("register")]
        public async Task<ActionResult<List<User>>> Register(UserDto request)
        {
            if (request != null)
            {
                string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);

                user.UserName = request.UserName;
                user.PasswordHash = passwordHash;
                user.Language = request.Language;

                appDbContext.Users.Add(user);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Users.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(
                x => x.UserName == request.UserName
                );

            var employee = await appDbContext.Employees.FirstOrDefaultAsync(
                x => x.UserId == user.UserId
                );


            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            if (employee != null)
            {
                var claims = ClaimsEmployee(user);
                string token = CreateToken(claims);
                return Ok(token);
            }
            else
            {
                var claims = ClaimsUser(user);
                string token = CreateToken(claims);
                return Ok(token);
            }
        }


        private List<Claim> ClaimsUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };
            return claims;
        }
        private List<Claim> ClaimsEmployee(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };
            return claims;
        }


        private string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
