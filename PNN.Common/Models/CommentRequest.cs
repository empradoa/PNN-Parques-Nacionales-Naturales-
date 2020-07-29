using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class CommentRequest
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Content { get; set; }

        public int zone { get; set; }

        public string User { get; set; }
    }
}
