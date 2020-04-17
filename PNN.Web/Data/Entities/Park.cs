using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PNN.web.Data.Entities;

namespace PNN.Web.Data.Entities
{
    public class Park
    {
        public int Id { get; set; }

        //Nombre del parque
        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //descripción del parque
        [Display(Name = "Descripción")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        //Año de creación del parque
        [Display(Name = "Creación")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Creation { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://TDB.azurewebsites.net{ImageUrl.Substring(1)}";

        [Display(Name = "Estado")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Been { get; set; }

        [Display(Name = "Extensión")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Extension { get; set; }

        [Display(Name = "Altura")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Height { get; set; }

        [Display(Name = "Temperatura")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Temperature { get; set; }

        [Display(Name = "Flora")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Flora { get; set; }

        [Display(Name = "Fauna")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Wildlife { get; set; }

        [Display(Name = "Comunidades")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Communities { get; set; }

        //Like del parque
        [Display(Name = "Like")]
        public int Like { get; set; }

        //DisLike del parque
        [Display(Name = "DisLike")]
        public int DisLike { get; set; }


        //relacion entre tablas foreign keys
        public Manager Manager { get; set; }
        public Location Location { get; set; }


        //relacion entre content y park
        public ICollection<Content> Contents { get; set; }
        //relacion entre zone y park
        public ICollection<Zone> Zones { get; set; }
    }
}
