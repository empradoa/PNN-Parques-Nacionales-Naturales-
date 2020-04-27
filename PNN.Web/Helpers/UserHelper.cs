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

        //eliminar un usuario con email
        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return true;
            }

            var response = await _userManager.DeleteAsync(user);
            return response.Succeeded;
        }

        //actualizar el usuario
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        //estos son los datos que le pasan de user y password para para lidar el password
        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);// si es true bloquea por n numero de intentos
        }


    }
}
