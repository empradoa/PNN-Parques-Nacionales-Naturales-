using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class ImageRequest
    {
        public int Id { get; set; }

        public int ContentId { get; set; }

        public byte[] ImageArray { get; set; }
    }
}
