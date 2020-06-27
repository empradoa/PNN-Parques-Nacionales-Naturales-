using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Data.Entities
{
    public class Area
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Location Location { get; set; }
        
        public virtual Park Park { get; set; }

        public virtual Zone Zone { get; set; }

    }
}
