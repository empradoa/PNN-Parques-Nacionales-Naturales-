using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class Content
    {
        public int Id { get; set; }

        //descripción del contenido
        [Display(Name = "Publicación")]
        //[MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
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

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://TDB.azurewebsites.net{ImageUrl.Substring(1)}";

        //Like del contenido
        [Display(Name = "Like")]
        public int Like { get; set; }

        //DisLike del contenido
        [Display(Name = "DisLike")]
        public int DisLike { get; set; }

        //relacion entre tablas foreign keys
        public ContentType ContentType { get; set; }
        public Park Park { get; set; }        
        public Location Location { get; set; }
        public Owner Owner { get; set; }

        //relacion entre comment y content
        public ICollection<Comment> Comments { get; set; }

    }
}
