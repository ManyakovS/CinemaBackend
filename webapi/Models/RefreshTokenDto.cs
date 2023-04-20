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
    public class RefreshTokenDto
    {
        public string Token { get; set; } = String.Empty;

        public DateTime Created { get; set; }= DateTime.Now;

        public DateTime Expires { get; set; }

    }



}
