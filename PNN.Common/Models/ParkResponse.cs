using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class ParkResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creation { get; set; }
        public string ImageUrl { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? "http://colombianp-001-site1.gtempurl.com/img/noimg.png"
            : $"http://colombianp-001-site1.gtempurl.com/{ImageUrl.Substring(1)}";

        public string Been { get; set; }

        public string Extension { get; set; }

        public string Height { get; set; }

        public string Temperature { get; set; }

        public string Flora { get; set; }

        public string Wildlife { get; set; }

        public string Communities { get; set; }

        public int Like { get; set; }

        public int DisLike { get; set; }


        //relacion entre tablas foreign keys
        public ManagerResponse Manager { get; set; }
        public ICollection<AreaResponse> Location { get; set; }


        //relacion entre content y park
        public ICollection<ContentResponse> Contents { get; set; }
        //relacion entre zone y park
        public ICollection<ZoneResponse> Zones { get; set; }
    }
}
