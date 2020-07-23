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
        [Route("GetContentsAsync")]
        public async Task<ActionResult> GetContentsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var prk = await _dataContext.Parks
                            .Include(z => z.Zones)
                            .Include(c => c.Contents)
                            .ThenInclude(cm => cm.Comments)
                            .Include(m => m.Manager)
                            .ThenInclude(u => u.User)
                            .Include(c => c.Locations)
                            .DefaultIfEmpty()
                            .ToListAsync();

            var cnt = await _dataContext.Contents
                            .Include(cm => cm.Comments)
                            .Include(c => c.ContentType)
                            .Include(u => u.User).ToListAsync();

            var area = await _dataContext.Areas
                                        .Include(a => a.Park)
                                        .Include(a => a.Location)
                                        .Include(a => a.Zone)
                                        .ToListAsync();

            var response = new PublicationsResponse
            {
                Parks = _converterHelper.ToListParkResponse(prk),
                Contents = _converterHelper.ToListContentResponse(cnt),
                Areas = _converterHelper.ToListAreaResponse(area)
            };


            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

    }
}