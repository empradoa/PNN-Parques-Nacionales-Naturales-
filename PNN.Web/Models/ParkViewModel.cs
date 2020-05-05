using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PNN.Web.Data.Entities;

namespace PNN.Web.Models
{
    public class ParkViewModel : Park
    {
        public int ManagerId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

    }
}
