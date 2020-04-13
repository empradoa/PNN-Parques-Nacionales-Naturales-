using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PNN.Web.Data.Entities
{
    public class ContentType
    {
        public int Id { get; set; }

        //Nombre de owner para ingresar a la app
        [Display(Name = "Tipo de Contenido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //relacion entre la tabla Contents y ContentType
        public ICollection<Content> Contents { get; set; }

    }
    
}
