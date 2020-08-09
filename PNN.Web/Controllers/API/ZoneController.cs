using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.Common.Models;
using PNN.web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ZoneController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ZoneController(DataContext context)
        {
            _dataContext = context;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutLikes([FromRoute] int id, [FromBody] ZoneRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldZone = await _dataContext.Zones.Include(p => p.Manager)
                                                  .Include(p => p.ZoneType)
                                                  .Include(p => p.Comments)
                                                  .Include(p => p.Locations)
                                                  .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (oldZone == null)
            {
                return BadRequest("La Zona No Existe.");
            }
            

            oldZone.Like = request.like;
            oldZone.DisLike = request.dislike;


            _dataContext.Zones.Update(oldZone);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }


    }
}
