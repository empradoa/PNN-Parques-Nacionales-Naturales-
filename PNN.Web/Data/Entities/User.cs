using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PNN.Web.Data.Entities
{
    public class User : IdentityUser
    {
        
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        //Apellidos del owner
        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        //Numero de celular del owner
        [Display(Name = "Celular")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string CellPhone { get; set; }

        //direccion del owner
        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Address { get; set; }

        [Display(Name = "Alias")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Alias { get; set; }

        [Display(Name = "Nombres completos")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Nombres completos")]
        public string FullNameWithUsername => $"{FirstName} {LastName} - {UserName}";

        //relacion entre User y comment
        public ICollection<Comment> Comments { get; set; }

        //relacion entre User y content
        public ICollection<Content> Contents { get; set; }
    }
}
