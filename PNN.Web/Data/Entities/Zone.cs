using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class Zone
    {
        //propiedad obligatoria para la tabla numero del zone
        public int Id { get; set; }

        //Nombre de la zona 
        [Display(Name = "Nombre")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://TDB.azurewebsites.net{ImageUrl.Substring(1)}";

        //descripción de la zona 
        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }      

        //Like de la zona
        [Display(Name = "Like")]
        public int Like { get; set; }

        //DisLike de la zona
        [Display(Name = "DisLike")]
        public int DisLike { get; set; }

        //relacion entre tablas foreign keys
        public ZoneType ZoneType { get; set; }
        public Location Location { get; set; }
        public Park Park { get; set; }

        //relacion entre zone y comment
        public ICollection<Comment> Comments { get; set; }

    }
}
