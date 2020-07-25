using PNN.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Models
{
    public class ZoneViewModel: Zone
    {
        public string latitud { get; set; }

        public string longuitud { get; set; }
    }
}
