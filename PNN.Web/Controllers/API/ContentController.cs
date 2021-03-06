﻿using System;
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
using PNN.Common.Helpers;
using System.IO;

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
        public async Task<IActionResult> PostContent([FromBody] ContentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dataContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return BadRequest("Usuario No Valido.");
            }

            var contentType = await _dataContext.ContentTypes.FindAsync(request.ContentType);
            if (contentType == null)
            {
                return BadRequest("Tipo De Publicacion No Valido.");
            }

            var park = await _dataContext.Parks.FindAsync(request.Park);
            if (park == null)
            {
                return BadRequest("Parque No Valido.");
            }

            var content = new Content
            {
                Date = request.Date,
                Description = request.Description,
                ContentType = contentType,
                Park = park,
                User = user
            };

            _dataContext.Contents.Add(content);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        [Route("AddImageToContent")]
        public async Task<IActionResult> AddImageToContent([FromBody] ImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var content = await _dataContext.Contents.FindAsync(request.ContentId);
            if (content == null)
            {
                return BadRequest("Publicacion No Valida.");
            }

            var imageUrl = string.Empty;
            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Contents";
                var fullPath = $"~/images/Contents/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            content.ImageUrl = imageUrl;

            _dataContext.Contents.Update(content);
            await _dataContext.SaveChangesAsync();
            return Ok(content);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutContent([FromRoute] int id, [FromBody] ContentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldContent = await _dataContext.Contents.Include(c=> c.User).FirstOrDefaultAsync(c => c.Id==request.Id);
            if (oldContent == null)
            {
                return BadRequest("La Publicacion No Existe.");
            }

            if (request.UserId != oldContent.User.Id) 
            {
                return BadRequest("El Usuario No es el Propietario De La Publicacion.");
            }

            var contentType = await _dataContext.ContentTypes.FindAsync(request.ContentType);
            if (contentType == null)
            {
                return BadRequest("El Tipo De Publicacion no Es Valido.");
            }

            var park = await _dataContext.Parks.FindAsync(request.Park);
            if (park == null)
            {
                return BadRequest("Parque No Existe.");
            }

            oldContent.Description = request.Description;
            oldContent.Date = request.Date;
            oldContent.ContentType = contentType;
            oldContent.Park = park;
            oldContent.User = await _dataContext.Users.FindAsync(request.UserId);

            _dataContext.Contents.Update(oldContent);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
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


        [HttpGet("GetLastContentByUserId/{id}")]
        public async Task<IActionResult> GetLastContentByUserId([FromRoute] String id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dataContext.Users
                .Include(u => u.Contents)
                .ThenInclude(c => c.ContentType)
                .Include(u => u.Contents)
                .ThenInclude(c => c.Park)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var content = user.Contents.LastOrDefault();
            var response = new ContentResponse
            {   
                Comments = _converterHelper.ToListCommentsResponse(content.Comments),
                ContentType = _converterHelper.ToContentTypeResponse(content.ContentType),
                Date = content.Date,
                Description = content.Description,
                Id = content.Id,
                ImageUrl = content.ImageUrl,
                Like = content.Like,
                Park = content.Park.Name,
                FullName = user.FullName,
                UserAlias = user.Alias
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var content = await _dataContext.Contents
                .Include(c => c.ContentType)
                .Include(c => c.Park)
                .Include(c => c.Comments)
                .Include(c=> c.Location)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (content == null)
            {
                return this.NotFound();
            }

            if (content.Comments.Count != 0)
            {
                foreach (var cmm in content.Comments)
                {
                    _dataContext.Comments.Remove(cmm);
                }
            }

            if (content.Location != null)
            {
                _dataContext.Locations.Remove(content.Location);
            }

            _dataContext.Contents.Remove(content);
            await _dataContext.SaveChangesAsync();
            return Ok("Publicacion Eliminada!");
        }


        [HttpPut]
        [Route("LikeCommentAsync/{id}")]
        public async Task<IActionResult> LikeComment([FromRoute] int id, [FromBody] LikeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.ContentId)
            {
                return BadRequest();
            }

            var oldcontent = await _dataContext.Contents.Include(p => p.Park)
                                                  .Include(p => p.ContentType)
                                                  .Include(p => p.Comments)
                                                  .Include(p => p.Location)
                                                  .Include(p => p.User)
                                                  .FirstOrDefaultAsync(c => c.Id == request.ContentId);
            if (oldcontent == null)
            {
                return BadRequest("la Publicacion No Existe.");
            }

            var comment = oldcontent.Comments.FirstOrDefault(c => c.Id == request.CommentId);

            comment.Like += request.Like;
            


            _dataContext.Comments.Update(comment);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }

    }
}