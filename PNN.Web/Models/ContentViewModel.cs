using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PNN.Web.Data.Entities;

namespace PNN.Web.Models
{
    public class ContentViewModel : Content
    {
        public string UserId { get; set; }
        //public IEnumerable<SelectListItem> Users { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo de Contenido")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciones un tipo de contenido")]
        public int ContentTypeId { get; set; }

        [Display(Name = "Parque")]
        [Range(0, int.MaxValue, ErrorMessage = "Desea especificar un parque natural")]
        public int ParkId { get; set; }

        //public int LocationId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> ContentTypes { get; set; }
        public IEnumerable<SelectListItem> Parks { get; set; } 
    }
}
