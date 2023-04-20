using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
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
        [HttpPost(Name = "PostFilm"), Authorize(Roles = "Admin")]
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


        //Get all films
        [HttpGet("film/all"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Film>>> GetAllFilm()
        {
            var films = await appDbContext.Films.ToListAsync();
            return Ok(films);
        }


        //Get FilmNow and FilmFuture films
        [HttpGet(Name = "GetAllFilmForViews"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Film>>> GetAllFilmForViews()
        {
            var films = await appDbContext.Films.ToListAsync();

            List<Film> filmNow = new List<Film>();
            List<Film> filmFuture = new List<Film>();

            foreach (var film in films)
            {
                if (film.RentalStartDate <= DateTime.Now && film.RentalEndtDate >= DateTime.Now.AddHours(6))
                    filmNow.Add(film);
                else if (film.RentalStartDate >= DateTime.Now.AddHours(6) && film.RentalEndtDate >= DateTime.Now.AddHours(6))
                    filmFuture.Add(film);
            };

            var filmReturn = new Dictionary<int, List<Film>>()
            {
                { 0, filmNow },
                { 1, filmFuture},
            };

            return Ok(filmReturn);
        }


        //Read single film
        [HttpGet("film/{id:int}", Name = "GetFilm"), Authorize(Roles = "User")]

        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await appDbContext.Films.FirstOrDefaultAsync(x => x.FilmId == id);
            if(film != null)
            {
                return Ok(film);
            }
            return NotFound("Film is not avaiable");
        }

        //Read single film
        [HttpGet("filmName/"), Authorize(Roles = "User")]

        public async Task<ActionResult<Film>> GetFilmForFilmName(string filmName)
        {
            var film = await appDbContext.Films.FirstOrDefaultAsync(x => x.Name == filmName);
            if (film != null)
            {
                return Ok(film);
            }
            return NotFound("Film is not avaiable");
        }

        //Update user

        [HttpPut(Name = "PutFilm"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Film>> UpdateFilm(Film updatedFilm)
        {
            if(updatedFilm != null)
            {
                var film = await appDbContext.Films.FirstOrDefaultAsync(e => e.FilmId == updatedFilm.FilmId);
                film!.Name = updatedFilm.Name;
                film.Duration = updatedFilm.Duration;
                film.Description = updatedFilm.Description;
                film.RentalEndtDate = updatedFilm.RentalEndtDate;
                film.RentalStartDate = updatedFilm.RentalStartDate;
                await appDbContext.SaveChangesAsync();
                return Ok(film);
            }
            return BadRequest("User not found");
        }

        //Delete user

        [HttpDelete(Name = "DeleteFilm"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<Film>>> DeleteFilm(int id)
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


        //Get all Tickets
        [HttpGet("genres/{filmID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetGenresForFilm(int filmID)
        {
            var genres = await appDbContext.Genres.FromSqlRaw($"GetGenresForFilm {filmID}").ToListAsync();

            return Ok(genres);
        }

        //Get all filmWorker
        [HttpGet("actors/{filmID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetFilmWorkersForFilm(int filmID)
        {

            var filmWorker = await appDbContext.FilmWorkers.FromSqlRaw($"GetFilmWorkersForFilm {filmID}").ToListAsync();

            return Ok(filmWorker);
        }


    }
}
