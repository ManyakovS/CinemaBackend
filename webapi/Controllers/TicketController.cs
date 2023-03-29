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
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public TicketController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create
        [HttpPost(Name = "PostTicket"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Ticket>>> AddTicket(Ticket newTicket)
        {
            if (newTicket != null)
            {
                appDbContext.Tickets.Add(newTicket);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Tickets.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }


        //Get all Tickets
        [HttpGet(Name = "GetAllTicket"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetAllTickets()
        {
            var tickets = await appDbContext.Tickets.FromSqlRaw("GetTickets").ToListAsync();

            return Ok(tickets);
        }

        //Get all Tickets
        [HttpGet("{SessionID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetAllTicketsForSession(int SessionID)
        {
            var tickets = await appDbContext.Tickets.FromSqlRaw($"GetTicketsForSession {SessionID}").ToListAsync();

            return Ok(tickets);
        }




        //Get all Tickets for State // Created // Bought // Booked // Returned
        [HttpGet("{SessionID}/{State}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetCreatedTicketsForSession(int SessionID, string State)
        {
            var tickets = await appDbContext.Tickets.FromSqlRaw($"GetTicketsForSessionAndState {SessionID}, {State}").ToListAsync();

            return Ok(tickets);
        }


        ////Read single film
        //[HttpGet("{id:int}", Name = "GetTicket"), Authorize(Roles = "User")]

        //public async Task<ActionResult<Ticket>> GetTicket(int id)
        //{
        //    var ticket = await appDbContext.Tickets.FirstOrDefaultAsync(x => x.TicketId == id);
        //    if (ticket != null)
        //    {
        //        return Ok(ticket);
        //    }
        //    return NotFound("Film is not avaiable");
        //}

        //Update user

        [HttpPut(Name = "PutTicket"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Ticket>> UpdateTicket(int TicketID, string state, int SessionID, int PlaceID, int EmployeeID, int UserID)
        {
            if(TicketID == 0 )
                return NotFound("TicketID not entered");
            var ticket = await appDbContext.Tickets.FirstOrDefaultAsync(e => e.TicketId == TicketID);

            if (state != null)
                ticket.State = state;
            if (SessionID != 0)
                ticket.SessionId = SessionID;
            if (PlaceID != 0)
                ticket.PlaceId = PlaceID;
            if (EmployeeID != 0) 
                ticket.EmployeeId = EmployeeID;

            await appDbContext.SaveChangesAsync();
            return Ok(ticket);
        }

        //Delete user

        [HttpDelete(Name = "DeleteTicket"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<Ticket>>> DeleteTicket(int id)
        {
            var ticket = await appDbContext.Tickets.FirstOrDefaultAsync(e => e.TicketId == id);
            if (ticket != null)
            {
                appDbContext.Tickets.Remove(ticket);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Tickets.ToListAsync());
            }
            return NotFound();
        }

    }
}
