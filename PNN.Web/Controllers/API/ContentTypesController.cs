using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContentTypesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ContentTypesController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public IEnumerable<ContentType> GetContentTypes()
        {
            return _dataContext.ContentTypes.OrderBy(ct => ct.Name);
        }
    }
}
