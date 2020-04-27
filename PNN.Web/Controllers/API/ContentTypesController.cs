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

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContentTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public ContentTypesController(DataContext context)
        {
            _context = context;
        }

        // trae la lista de contentType: api/ContentTypes
        [HttpGet]
        public IEnumerable<ContentType> GetContentTypes()
        {
            return _context.ContentTypes;
        }
    }
}