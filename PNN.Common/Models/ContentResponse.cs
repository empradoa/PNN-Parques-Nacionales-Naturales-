using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class ContentResponse
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl =>ImageUrl == null ?
                    "http://colombianp-001-site1.gtempurl.com/img/noimg.png"
                    : ImageUrl;
            
        public int Like { get; set; }
        public ContentTypeResponse ContentType { get; set; }
        public string Park { get; set; }

        public ICollection<CommentResponse> Comments { get; set; }
    }
}
