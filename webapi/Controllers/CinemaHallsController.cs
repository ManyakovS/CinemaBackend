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
    public class CinemaHallsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CinemaHallsController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create session
        [HttpPost(Name = "PostCinemaHall"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CinemaHall>>> AddCinemaHalln(CinemaHall newCinemaHall)
        {
            if (newCinemaHall != null)
            {
                appDbContext.CinemaHalls.Add(newCinemaHall);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.CinemaHalls.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }


        //Get all sessions
        [HttpGet(Name = "GetAllCinemaHall"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<CinemaHall>>> GetAllCinemaHalln()
        {
            var cinemaHall = await appDbContext.CinemaHalls.ToListAsync();
            return Ok(cinemaHall);
        }


        //Read single session
        [HttpGet("cinemaHall/{id:int}", Name = "GetCinemaHall"), Authorize(Roles = "User")]

        public async Task<ActionResult<CinemaHall>> GetCinemaHall(int id)
        {
            var cinemaHall = await appDbContext.CinemaHalls.FirstOrDefaultAsync(x => x.CinemaHallId == id);
            if (cinemaHall != null)
            {
                return Ok(cinemaHall);
            }
            return NotFound("Film is not avaiable");
        }

        //Get all Tickets
        [HttpGet("Session/{SessionID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetCinemaHallForSession(int SessionID)
        {
            var cinemaHall = await appDbContext.CinemaHalls.FromSqlRaw($"GetTicketsForSession {SessionID}").ToListAsync();

            return Ok(cinemaHall);
        }

        //Update session

        [HttpPut(Name = "PutCinemaHall"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<CinemaHall>> UpdateCinemaHall(CinemaHall updatedCinemaHal)
        {
            if (updatedCinemaHal != null)
            {
                var session = await appDbContext.CinemaHalls.FirstOrDefaultAsync(e => e.CinemaHallId == updatedCinemaHal.CinemaHallId);

                if (updatedCinemaHal.Name != null)
                    session.Name = updatedCinemaHal.Name;

                if (updatedCinemaHal.NumberOfSeats != null)
                    session.NumberOfSeats = updatedCinemaHal.NumberOfSeats;

                await appDbContext.SaveChangesAsync();
                return Ok(session);
            }
            return BadRequest("User not found");
        }

        //Delete session

        [HttpDelete(Name = "DeleteCinemaHall"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<CinemaHall>>> DeleteCinemaHall(int id)
        {
            var cinemaHall = await appDbContext.CinemaHalls.FirstOrDefaultAsync(e => e.CinemaHallId == id);
            if (cinemaHall != null)
            {
                appDbContext.CinemaHalls.Remove(cinemaHall);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.CinemaHalls.ToListAsync());
            }
            return NotFound();
        }

        //Get all Places
        [HttpGet("places/{CinemaHallID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetSeats(int CinemaHallID)
        {

            var places = await appDbContext.Places.FromSqlRaw($"GetSeats {CinemaHallID}").ToListAsync();

            return Ok(places);
        }

        //Get place
        [HttpGet("place/{PlaceId:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetSeat(int PlaceId)
        {

            var places = await appDbContext.Places.FromSqlRaw($"GetSeatsForId {PlaceId}").ToListAsync();

            return Ok(places);
        }

    }
}
