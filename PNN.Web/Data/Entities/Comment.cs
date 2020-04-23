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
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        //fecha de la publicación del cotenido
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateLocal => Date.ToLocalTime();

        //Like de los comentarios
        [Display(Name = "Like")]
        public int Like { get; set; }

        //DisLike de los comentarios
        [Display(Name = "DisLike")]
        public int DisLike { get; set; }

        //relacion entre tablas foreign keys
        public Zone Zone { get; set; }
        public Content Content { get; set; }
        public User User { get; set; }
    }
}
