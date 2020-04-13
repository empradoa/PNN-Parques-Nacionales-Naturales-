using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class Location
    {
        //propiedad obligatoria para la tabla
        public int Id { get; set; }

        //Latitud 
        [Display(Name = "Latitud")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Latitude { get; set; }

        //Longitud
        [Display(Name = "Longitud")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Longitude { get; set; }


        //relacion entre Location y (content, zone, park and owner)
        public ICollection<Content> Contents { get; set; }
        public ICollection<Zone> Zones { get; set; }
        public ICollection<Park> Parks { get; set; }
        public ICollection<Owner> Owners { get; set; }
    }
}
