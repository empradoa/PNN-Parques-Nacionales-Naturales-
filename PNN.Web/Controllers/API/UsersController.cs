﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.Common.Models;
using PNN.web.Data;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
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
                .ThenInclude(z=> z.Zones)
                .Include(u => u.Contents)
                .ThenInclude(ct => ct.Comments)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == emailRequest.Email.ToLower());

            var response = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                CellPhone = user.CellPhone,
                Contents = user.Contents.Select(ct => new ContentResponse
                {
                    Date = ct.Date,
                    Id = ct.Id,
                    ImageUrl = ct.ImageFullPath,
                    Description = ct.Description,
                    Like = ct.Like,
                    ContentType =  new ContentTypeResponse 
                    {
                        Id = ct.ContentType.Id,
                        Name = ct.ContentType.Name
                    },
                    Park = ct.Park.Name,
                    Comments = ct.Comments.Select(cm => new CommentResponse
                    {
                        Date = cm.Date,
                        Description = cm.Description,
                        Id = cm.Id,
                        Like = cm.Like
                    }).ToList()
                }).ToList()
            };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
