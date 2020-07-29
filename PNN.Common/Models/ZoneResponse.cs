using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class ZoneResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImageUrl { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? "http://colombianp-001-site1.gtempurl.com/img/Noimgpark.png"
            : $"http://colombianp-001-site1.gtempurl.com/{ImageUrl.Substring(1)}";

        public string Description { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public ZoneTypesResponse ZoneType { get; set; }
        public ICollection<AreaResponse> Location { get; set; }
        public int ManagerId { get; set; }
        public String Manager { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
    }
}
