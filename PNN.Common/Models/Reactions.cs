using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class Reactions
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public int ContentId { get; set; }

        public int ZoneId { get; set; }

        public int ParkId { get; set; }

        public byte Tipo { get; set; }  //1: dislike  2:Like

        public String UserId { get; set; }
    }
}
