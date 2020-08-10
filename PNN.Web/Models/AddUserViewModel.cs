using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PNN.Web.Models
{
    public class AddUserViewModel : EditUserViewModel
    {

        [Display(Name = "Alias")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        Random rnd = new Random();
       // 'AddUserViewModel.Alias' oculta el miembro heredado 'EditUserViewModel.Alias'. Use la palabra clave new si su intención era ocultarlo.
        public string Alias => $"{FirstName.Trim().Replace(" ", string.Empty)}_{rnd.Next(0, 100)}".ToLower();
       // 'AddUserViewModel.Alias' oculta el miembro heredado 'EditUserViewModel.Alias'. Use la palabra clave new si su intención era ocultarlo.

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [EmailAddress]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        public string Password { get; set; }

        [Display(Name = "Confirmar Password")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

       
        [Display(Name = "Registrar Como")]
        [Range(1,int.MaxValue,ErrorMessage ="Debe Seleccionar un rol")]
        public int RoleId { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }

}
