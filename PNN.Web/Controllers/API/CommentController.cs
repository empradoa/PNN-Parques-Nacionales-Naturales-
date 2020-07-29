using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PNN.Common.Models;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;


        public CommentController(DataContext dataContext, 
                                IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper; 
        }




        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CommentRequest request)
        {
            Content c= default; Zone z = default;

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            if (request.Content != 0)
            {
                c = _dataContext.Contents.Find(request.Content);
            }

            if (request.zone != 0)
            {
                z = _dataContext.Zones.Find(request.zone);
            }

            var comment = new Comment {
                                Description = request.Description,
                                Date = request.Date.ToUniversalTime(),
                                Content = c,
                                Zone = z,
                                User = _dataContext.Users.Find(request.User)
                                };

            if (comment == null)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Error en la Estructura del comentario, intente nuevamente."
                });
            }


            // Se enlaza el modelo dentro del proxie de EFcore para que no genere error al insertar
            _dataContext.Attach(comment.User);
            _dataContext.Attach(comment.Content);

            var result = _dataContext.Comments.Add(comment);
            await _dataContext.SaveChangesAsync();



            if (result == null )
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Error en la Estructura del comentario, comuniquese con el Area de sistemas"
                });
            }

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "The Comment has created successfully!"
            });
        }

    }
}
 