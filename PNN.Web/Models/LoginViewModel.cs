using System.ComponentModel.DataAnnotations;

namespace PNN.Web.Models
{
    //este es el modelo que controla el fronted de login
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo usuario debe ser una dirección de correo electrónico válida.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //valida que la clave no sea minima de 6 digitos
        [MinLength(6, ErrorMessage = "El campo Password no debe ser menor a 6 digítos.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
