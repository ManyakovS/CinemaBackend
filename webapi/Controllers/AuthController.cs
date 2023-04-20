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
using System.Security.Cryptography;
using static DevExpress.Data.Helpers.FindSearchRichParser;
using Azure.Core;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DevExpress.Internal;

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
                user.Email = request.UserName;

                appDbContext.Users.Add(user);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Users.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }

        [HttpGet(Name = "isAdmin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> isAdmin()
        {
            return Ok(true);
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

            List<Claim> claims;
            if (employee != null)
                claims = ClaimsEmployee(user);
            else
                claims = ClaimsUser(user);

            string token = CreateToken(claims);


            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);
            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenCreated = refreshToken.Created;
            user.RefreshTokenExpires = refreshToken.Expires;
            await appDbContext.SaveChangesAsync();

            UserDto userDto = new UserDto(user);
            userDto.Token = token;
            userDto.RefreshToken = refreshToken.Token;

            return Ok(userDto);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(string refreshToken = "")
        {
            if(refreshToken == "")
                refreshToken = Request.Cookies["refreshToken"];

            var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken));

            if(user == null)
            {
                return Unauthorized("Invalid Refresh Token. Dont found user");
            }

            if(!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(user.RefreshTokenExpires < DateTime.Now)
            {
                return Unauthorized("Invalid expired.");
            }

            List<Claim> claims;
            if (employee != null)
                claims = ClaimsEmployee(user);
            else
                claims = ClaimsUser(user);

            string token = CreateToken(claims);

            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenCreated = newRefreshToken.Created;
            user.RefreshTokenExpires = newRefreshToken.Expires;
            await appDbContext.SaveChangesAsync();

            UserDto userDto = new UserDto(user);
            userDto.Token = token;
            userDto.RefreshToken = newRefreshToken.Token;

            return Ok(userDto);
        }

        private RefreshTokenDto GenerateRefreshToken()
        {
            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddDays(15)
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshTokenDto
            {
                Token = jwt,
                Expires = DateTime.Now.AddDays(15),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshTokenDto newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);



            //user.RefreshToken = newRefreshToken.Token;
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
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
