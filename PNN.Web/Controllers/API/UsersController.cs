using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.Common.Models;
using PNN.web.Data;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUser(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _dataContext.Users
                            .Include(u => u.Contents)
                            .Include(u => u.Comments)
                            .FirstOrDefaultAsync(u => u.Email == emailRequest.Email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
