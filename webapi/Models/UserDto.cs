using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.EntityFrameworkCore;
using webapi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace webapi.Models
{
    public class UserDto
    {
        [Key]
        public long UserId { get; set; }

        public string Password { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

    }



}
