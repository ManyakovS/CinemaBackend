using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using webapi.Data;
using webapi.Models;
using static System.Collections.Specialized.BitVector32;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        private readonly AppDbContext appDbContext;

        public SessionController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create session
        [HttpPost(Name = "PostSession(Date format:Month Day Year)"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Session>>> AddSession(string Date, string Time, string TimeEnd, int FilmId, int CinemaHallId)
        {
            if (DateTime.Parse(Date) >= DateTime.UtcNow && FilmId != 0 && CinemaHallId != 0)
            {
                if(TimeSpan.Parse(Time) > TimeSpan.Parse(TimeEnd))
                    return BadRequest("Invalid time");

                Session? newSession = new Session(Date, Time, TimeEnd);

                Film? _film = await appDbContext.Films.FirstOrDefaultAsync(e => e.FilmId == FilmId);
                if (_film != null)
                {
                    newSession.FilmId = FilmId;
                    newSession.film = _film;
                }
                else
                    return BadRequest("Invalid film id");

                CinemaHall? _cinemaHall = await appDbContext.CinemaHalls.FirstOrDefaultAsync(e => e.CinemaHallId == CinemaHallId);
                if (_cinemaHall != null)
                {
                    newSession.CinemaHallId = CinemaHallId;
                    newSession.cinemaHall = _cinemaHall;
                }
                else
                    return BadRequest("Invalid CinemaHall id");


                appDbContext.Sessions.Add(newSession);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Sessions.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }


        //Get all sessions
        [HttpGet(Name = "GetAllSession"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Session>>> GetAllSession()
        {
            var session = await appDbContext.Sessions.ToListAsync();
            return Ok(session);
        }


        //Read single session
        [HttpGet("session/{id:int}", Name = "GetSession"), Authorize(Roles = "User")]

        public async Task<ActionResult<Session>> GetSession(int id)
        {
            var session = await appDbContext.Sessions.FirstOrDefaultAsync(x => x.SessionId == id);
            if (session != null)
            {
                return Ok(session);
            }
            return NotFound("Film is not avaiable");
        }

        //Get all Session for film
        [HttpGet("films/{FilmID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetSessionsForFilm(int FilmID)
        {
            var session = await appDbContext.Sessions.FromSqlRaw($"GetSessionsForFilm {FilmID}").ToListAsync();

            return Ok(session);
        }

        //Update session

        [HttpPut(Name = "PutSession"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Session>> UpdateSession(
            int Sessionid,string Date, string Time, string TimeEnd, int FilmId, int CinemaHallId
            )

        {
            Session? session = await appDbContext.Sessions.FirstOrDefaultAsync(e => e.SessionId == Sessionid);
            if (session == null)
                BadRequest("User not found");

            if (Date != "")
                session.Date = DateTime.Parse(Date);
            else
                return BadRequest("Invalid Date");

            if (Time != "")
                session.Time = TimeSpan.Parse(Time);
            else
                return BadRequest("Invalid Time");

            if (TimeEnd != "")
                session.TimeEnd = TimeSpan.Parse(TimeEnd);
            else
                return BadRequest("Invalid TimeEnd");

            Film? _film = await appDbContext.Films.FirstOrDefaultAsync(e => e.FilmId == FilmId);
            if (_film != null)
            {
                session.FilmId = FilmId;
                session.film = _film;
            }
            else
                return BadRequest("Invalid film id");

            CinemaHall? _cinemaHall = await appDbContext.CinemaHalls.FirstOrDefaultAsync(e => e.CinemaHallId == CinemaHallId);
            if (_cinemaHall != null)
            {
                session.CinemaHallId = CinemaHallId;
                session.cinemaHall = _cinemaHall;
            }
            else
                return BadRequest("Invalid CinemaHall id");

            await appDbContext.SaveChangesAsync();
            return Ok(session);

        }

        //Delete session

        [HttpDelete(Name = "DeleteSession"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<Session>>> DeleteSession(int id)
        {
            var session = await appDbContext.Sessions.FirstOrDefaultAsync(e => e.SessionId == id);
            if (session != null)
            {
                appDbContext.Sessions.Remove(session);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Sessions.ToListAsync());
            }
            return NotFound();
        }



    }
}
