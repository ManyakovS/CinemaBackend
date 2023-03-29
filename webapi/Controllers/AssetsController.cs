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
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public AssetsController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        //Create
        [HttpPost(Name = "PostAsset"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Assets>>> AddAsset(Assets newAssets)
        {
            if (newAssets != null)
            {
                appDbContext.Assets.Add(newAssets);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Assets.ToListAsync());
            }
            return BadRequest("Object instance not set");
        }


        //Get all films
        [HttpGet(Name = "GetAllAsset"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Assets>>> GetAllAsset()
        {
            var assets = await appDbContext.Assets.ToListAsync();
            return Ok(assets);
        }


        //Read single film
        [HttpGet("assets/{id:int}", Name = "GetAsset"), Authorize(Roles = "User")]

        public async Task<ActionResult<Assets>> GetAsset(int id)
        {
            var asset = await appDbContext.Films.FirstOrDefaultAsync(x => x.FilmId == id);
            if (asset != null)
            {
                return Ok(asset);
            }
            return NotFound("Film is not avaiable");
        }

        //Update user

        [HttpPut(Name = "PutAsset"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Assets>> UpdateAsset(Assets updatedAsset)
        {
            if (updatedAsset != null)
            {
                var asset = await appDbContext.Assets.FirstOrDefaultAsync(e => e.AssetsId == updatedAsset.AssetsId);
                asset!.Link = updatedAsset!.Link;
                asset.Description = updatedAsset.Description;

                await appDbContext.SaveChangesAsync();
                return Ok(asset);
            }
            return BadRequest("User not found");
        }

        //Delete user

        [HttpDelete(Name = "DeleteAsset"), Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<Assets>>> DeleteAsset(int id)
        {
            var asset = await appDbContext.Assets.FirstOrDefaultAsync(e => e.AssetsId == id);
            if (asset != null)
            {
                appDbContext.Assets.Remove(asset);
                await appDbContext.SaveChangesAsync();
                return Ok(await appDbContext.Assets.ToListAsync());
            }
            return NotFound();
        }



        //Get all Assets for film
        [HttpGet("films/{filmID:int}"), Authorize(Roles = "User")]

        public async Task<ActionResult<List<Ticket>>> GetAllTicketsForSession(int filmID)
        {
            var assets = await appDbContext.Assets.FromSqlRaw($"GetAssetsForFilm {filmID}").ToListAsync();

            return Ok(assets);
        }

    }
}
