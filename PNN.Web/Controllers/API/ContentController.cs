using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.Web.Data.Entities;
using PNN.web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PNN.Common.Models;
using PNN.Web.Helpers;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContentController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public ContentController(DataContext context, IConverterHelper converterHelper)
        {
            _dataContext = context;
            _converterHelper = converterHelper;
        }

        // trae la lista de contentType: api/ContentTypes
        [HttpGet]
        public IEnumerable<ContentType> GetContentTypes()
        {
            return _dataContext.ContentTypes;
        }

        [HttpPost]
        [Route("GetParks")]
        public async Task<ActionResult<IEnumerable<ParkResponse>>> GetParksAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var parks = await  _dataContext.Parks
                .Include(z => z.Zones)
                .Include(c => c.Contents)
                .ThenInclude(cm => cm.Comments).ToListAsync();

            var response = _converterHelper.ToListParkResponse(parks);
            

            if (parks == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}