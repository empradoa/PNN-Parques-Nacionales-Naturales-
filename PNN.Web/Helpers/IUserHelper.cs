﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

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

        //le pasamos una instancia de la loginViewModel y nos devuelve el SignInResult que nos dice si logueo o no logueo
        Task<SignInResult> LoginAsync(LoginViewModel model);

        //metodo para desloguear
        Task LogoutAsync();

        //Eliminar un usuario de la bd
        Task<bool> DeleteUserAsync(string email);

        //actualizar datos de usuario 
        Task<IdentityResult> UpdateUserAsync(User user);

        //validamos el logeo
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        //Agregar usuario Con role
        Task<User> AddUser(AddUserViewModel view, string role);

        //Cambiar Password
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    }
}
