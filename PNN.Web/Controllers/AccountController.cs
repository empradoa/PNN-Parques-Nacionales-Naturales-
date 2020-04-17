using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PNN.Web.Helpers;
using PNN.Web.Models;

namespace PNN.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        //inyectamos de la clase IUserHelper
        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //sobre cargamos el metodo login, para poder valide los datos en el formulario de login. Heredad de la LoginViewModel
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            //Devolvemos el modelo para que el usuario no pierda lo que digito
            //EL ModelState maneja el estado del controlador, los mensajes de errors del LoginViewModel.cs
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    //si no me logueo lo devuelvo a la vista index del controlador Home
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Usuario o password no valido");
                model.Password = string.Empty;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index","Home");
        }
    }

}
