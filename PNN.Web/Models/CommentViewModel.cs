using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PNN.Web.Data.Entities;

namespace PNN.Web.Models
{
    public class CommentViewModel : Content
    {
        public int ContentId { get; set; }
        public int ZoneId { get; set; }
        public String UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
