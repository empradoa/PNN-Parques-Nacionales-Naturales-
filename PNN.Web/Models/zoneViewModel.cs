using Microsoft.AspNetCore.Mvc.Rendering;
using PNN.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Models
{
    public class ZoneViewModel: Zone
    {
        public string latitud { get; set; }

        public string longuitud { get; set; }

        [Display(Name = "Tipo de Zona")]
        [Range(0, int.MaxValue, ErrorMessage = "Desea especificar un tipo de zona")]
        public int ZoneTypeId { get; set; }

        public IEnumerable<SelectListItem> ZoneTypes { get; set; }

        [Display(Name = "Parque")]
        [Range(0, int.MaxValue, ErrorMessage = "Desea especificar un parque natural")]

        
        public int ParkId { get; set; }
        

        public IEnumerable<SelectListItem> Parks { get; set; }
    }
}
