using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.Common.Models;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public UsersController(DataContext dataContext, IConverterHelper converterHelper,
                               IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserAsync(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var user = await _dataContext.Users
                .Include(u => u.Contents)
                .ThenInclude(ct => ct.ContentType)
                .Include(u => u.Contents)
                .ThenInclude(pk => pk.Park)
                .ThenInclude(z => z.Zones)
                .Include(c => c.Contents)
                .ThenInclude(cm => cm.Comments)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == emailRequest.Email.ToLower());

            var response = _converterHelper.ToUserResponse(user);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        

    }
}
