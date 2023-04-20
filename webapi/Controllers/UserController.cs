using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AppDbContext appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create
        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User newUser)
        {
            if (newUser != null)
            {   
                AuthOptions authOptions = new AuthOptions();
                newUser.Token = authOptions.GetToken(newUser.UserName).ToString();
                appDbContext.Users.Add(newUser);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Users.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }

        
        //Get all users
        [HttpGet, Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var users = await appDbContext.Users.ToListAsync();
            return Ok(users);
        }


        //Read single users
        [HttpGet("{id:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if(user != null)
            {
                return Ok(user);
            }
            return NotFound("User is not avaiable");
        }

        //Read single users
        [HttpGet("GetUserOnLogin"), Authorize(Roles = "User")]

        public async Task<ActionResult<User>> GetUserOnLogin(string UserName)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User is not avaiable");
        }

        //Update user

        [HttpPut, Authorize(Roles = "User")]
        public async Task<ActionResult<User>> UpdateUser(User updatedUser)
        {
            if(updatedUser != null)
            {
                var user = await appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == updatedUser.UserId);
                if (updatedUser.UserName != null)
                    user!.UserName = updatedUser.UserName;

                if (updatedUser.PasswordHash != null)
                    user!.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);

                if (updatedUser.Name != null)
                    user!.Name = updatedUser.Name;

                if (updatedUser.LastName != null)
                    user!.LastName = updatedUser.LastName;

                if (updatedUser.Email != null)
                    user!.Email = updatedUser.Email;

                if (updatedUser.Icon != null)
                    user.Icon = updatedUser.Icon;

                if (updatedUser.Theme != null)
                    user.Theme = updatedUser.Theme;

                if (updatedUser.Language != null)
                    user.Language = updatedUser.Language;

                if (updatedUser.CinemaAdress != null)
                    user.CinemaAdress = updatedUser.CinemaAdress;

                await appDbContext.SaveChangesAsync();
                return Ok(user);
            }
            return BadRequest("User not found");
        }

        //Delete user

        [HttpDelete, Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(e => e.UserId == id);
            if( user != null) 
            {
                appDbContext.Users.Remove(user);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Users.ToListAsync());
            }
            return NotFound();
        }

    }
}
