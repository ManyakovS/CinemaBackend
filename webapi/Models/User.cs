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
    public class User
    {
        [Key]
        public long UserId { get; set; }

        public string PasswordHash { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Icon { get; set; } = "Default icon";
        public string Theme { get; set; } = "System";
        public string Language { get; set; } = "Ru";
        public string CinemaAdress { get; set; } = "Default";
        public string Token { get; set; }
        public DateTime RegistratedDate { get; set; } = DateTime.Now;

    }



}
