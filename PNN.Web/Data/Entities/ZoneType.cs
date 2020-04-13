using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class ZoneType
    {
        public int Id { get; set; }

        //Nombre de tipo de zona
        [Display(Name = "Tipo de Zona")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Zone> Zones { get; set; }
    }
}
