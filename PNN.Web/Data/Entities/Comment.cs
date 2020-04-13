using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        //Nombre de owner para ingresar a la app
        [Display(Name = "Comentario")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        //Like de los comentarios
        [Display(Name = "Like")]
        public int Like { get; set; }

        //DisLike de los comentarios
        [Display(Name = "DisLike")]
        public int DisLike { get; set; }

        //relacion entre tablas foreign keys
        public Zone Zone { get; set; }
        public Content Content { get; set; }
        public Owner Owner { get; set; }
    }
}
