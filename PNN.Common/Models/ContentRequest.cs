using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class ContentRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ContentType { get; set; }
        public int Park { get; set; }
        public string UserId { get; set; }
       

    }
}
