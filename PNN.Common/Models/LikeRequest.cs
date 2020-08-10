using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class LikeRequest
    {
        public int Like { get; set; }
        public int ZoneId { get; set; }
        public int ContentId { get; set; }
        public int CommentId { get; set; }

    }
}
