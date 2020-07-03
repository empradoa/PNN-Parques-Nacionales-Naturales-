using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PNN.Common.Models
{
    public class AreaResponse
    {
        public int Id { get; set; }

        public virtual LocationResponse Location { get; set; }

        public int Park { get; set; }

        public int Zone { get; set; }

    }
}
