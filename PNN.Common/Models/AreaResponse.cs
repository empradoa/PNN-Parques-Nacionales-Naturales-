using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class AreaResponse
    {
        public int Id { get; set; }

        public virtual LocationResponse Location { get; set; }

        public virtual ParkResponse Park { get; set; }

        public virtual ZoneResponse Zone { get; set; }
    }
}
