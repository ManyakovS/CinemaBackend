using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {

        private readonly AppDbContext appDbContext;

        public FilmController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create
        [HttpPost]
        public async Task<ActionResult<List<Film>>> AddFilm(Film newFilm)
        {
            if (newFilm != null)
            {
                appDbContext.Films.Add(newFilm);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Films.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }

        
        //Get all users
        [HttpGet]

        public async Task<ActionResult<List<Film>>> GetAllFilm()
        {
            var films = await appDbContext.Films.ToListAsync();
            return Ok(films);
        }


        //Read single users
        [HttpGet("{id:int}")]

        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await appDbContext.Films.FirstOrDefaultAsync(x => x.FilmId == id);
            if(film != null)
            {
                return Ok(film);
            }
            return NotFound("User is not avaiable");
        }

        //Update user

        [HttpPut]
        public async Task<ActionResult<Film>> UpdateFilm(Film updatedFilm)
        {
            if(updatedFilm != null)
            {
                var film = await appDbContext.Films.FirstOrDefaultAsync(e => e.FilmId == updatedFilm.FilmId);
                film!.Name = updatedFilm.Name;
                film.Duration = updatedFilm.Duration;
                await appDbContext.SaveChangesAsync();
                return Ok(film);
            }
            return BadRequest("User not found");
        }

        //Delete user

        [HttpDelete]

        public async Task<ActionResult<List<Film>>> DeleteUser(int id)
        {
            var film = await appDbContext.Films.FirstOrDefaultAsync(e => e.FilmId == id);
            if(film != null) 
            {
                appDbContext.Films.Remove(film);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Films.ToListAsync());
            }
            return NotFound();
        }

    }
}
