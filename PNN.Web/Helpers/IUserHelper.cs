﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PNN.Web.Data.Entities;

namespace PNN.Web.Helpers
{
    public interface IUserHelper
    {
        //metodo que me devuelve el user cuando le envio el email
        Task<User> GetUserByEmailAsync(string email);

        //Crear nuevos usuarios, le enviamos user y password y nos devuelve un IdentityResult
        Task<IdentityResult> AddUserAsync(User user, string password);

        //verifica si existe un rol
        Task CheckRoleAsync(string roleName);

        //matricula un usuario a un rol
        Task AddUserToRoleAsync(User user, string roleName);

        //Valida si un usuario pertenece o no a un Rol
        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
