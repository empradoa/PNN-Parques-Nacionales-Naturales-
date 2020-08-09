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

    public class ParkController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ParkController(DataContext context)
        {
            _dataContext = context;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutLikes([FromRoute] int id, [FromBody] ParkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldPark = await _dataContext.Parks.Include(p => p.Manager)
                                                  .Include(p => p.Zones)
                                                  .Include(p => p.Contents)
                                                  .Include(p => p.Locations)
                                                  .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (oldPark == null)
            {
                return BadRequest("El Parque No Existe.");
            }



            oldPark.Like = request.like;
            oldPark.DisLike = request.dislike;
            

            _dataContext.Parks.Update(oldPark);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }


    }
}
