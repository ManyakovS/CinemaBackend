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

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;

        public string? Token { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;

        public UserDto() { }

        public UserDto(User user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Password = user.PasswordHash;
            Token = user.Token;
            RefreshToken = user.RefreshToken;
            Language = user.Language;
            Icon = user.Icon;
            Theme = user.Theme;
        }

    }



}
