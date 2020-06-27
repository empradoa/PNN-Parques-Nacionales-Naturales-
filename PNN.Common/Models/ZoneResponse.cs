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
            ? null
            : $"https://TDB.azurewebsites.net{ImageUrl.Substring(1)}";

        public string Description { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public ZoneTypesResponse ZoneType { get; set; }
        public ICollection<LocationResponse> Location { get; set; }
        public ManagerResponse Manager { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
    }
}
