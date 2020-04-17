﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

namespace PNN.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            //trabaja con usuarios personalizados
            SignInManager<User> signInManager)
        {

            //propiedades de solo lectura que see inicia en el constructor de la clase
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        //adicionar un nuevo usuario de forma asincrona
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        //Adicione el usuario al rol
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        //verifique si el rol existe
        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        //verifica si el correo existe
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        //verifica si el usuario pertenece o no al rol
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        //Implementación del metodo para loguearse declarado en el IUserHelper
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);//en caso de ir true debe implemetarse la funcionalidad para desbloquear usuarios bloqueados
        }

        //implementación del metodo para desloguear del IUserHelper
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}