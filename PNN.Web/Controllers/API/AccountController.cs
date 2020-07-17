using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMailHelper _mailHelper;

        public AccountController(DataContext dataContext, 
                               IConverterHelper converterHelper,
                               IUserHelper userHelper,
                               IMailHelper  mailHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
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

            Random rnd = new Random();
            // Obtiene un número natural (incluye el 0) aleatorio entre 0 e int.MaxValue
            int alt = rnd.Next(1000);

            User u = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Email = userRequest.Email,
                UserName = userRequest.Email,
                CellPhone = userRequest.CellPhone,
                Address = userRequest.Address,
                Alias = _userHelper.GenerateAlias(userRequest.FirstName,userRequest.LastName)
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
        [Route("RecoverPasswords")]
        public async Task<IActionResult> RecoverPasswords([FromBody] EmailRequest request) 
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

            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword","Account",new { token = myToken }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(user.Email, "ConParks Password Reset", $"<h1>ConParks Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
            


            return Ok( new Response<object> 
                        { 
                          IsSuccess= true,
                          Message = "The instructions to recover your password has been sent to email."
                        });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = await _userHelper.GetUserByEmailAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest("User not found.");
            }

            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.Address = request.Address;
            userEntity.CellPhone = request.CellPhone;
            
            var respose = await _userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            var updatedUser = await _userHelper.GetUserByEmailAsync(request.Email);
            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
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
                    Message = "This email is not assigned to any user."
                });
            }

            var result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return Ok(new Response<object>
            {
                IsSuccess = true,
                Message = "The password was changed successfully!"
            });
        }


    }
}