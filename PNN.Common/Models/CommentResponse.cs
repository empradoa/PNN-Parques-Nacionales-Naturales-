using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class CommentResponse
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Like { get; set; }
    }
}
