using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PNN.Common.Models;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;

namespace PNN.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public AccountController(DataContext dataContext, 
                               IConverterHelper converterHelper,
                               IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRequest userRequest)
        {
            //TODO: hacer la validacion por email y mover a un controlador API Account
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await _userHelper.GetUserByEmailAsync(userRequest.Email);

            if (user != null)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Este Correo ya esta Registrado"
                });
            }

            User u = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                UserName = userRequest.Email,
                CellPhone = userRequest.CellPhone,
                Address = userRequest.Address,
                Alias = "${userRequest.FirstName}_${userRequest.LastName}"

            };

            var result = await _userHelper.AddUserAsync(u, userRequest.Password);

            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var userNew = await _userHelper.GetUserByEmailAsync(userRequest.Email);

            await _userHelper.AddUserToRoleAsync(userNew, "Customer");

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "Registro Exitoso!"
            });
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await _userHelper.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = "Este Correo no ha Sido Asignado a Ningun Usuario."
                });
            }


            //TODO falta  crear el token de recuperacionde password y el envio del correo

            return Ok();
        }

    }
}