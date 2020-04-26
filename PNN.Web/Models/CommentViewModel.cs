using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PNN.Web.Data.Entities;

namespace PNN.Web.Models
{
    public class CommentViewModel : Content
    {
        public int ContentId { get; set; }
        public int ZoneId { get; set; }
        public string UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
