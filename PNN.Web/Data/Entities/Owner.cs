using System.Collections.Generic;
using PNN.Web.Data.Entities;
namespace PNN.Web.Data.Entities
{
public class Owner
    {
        //propiedad obligatoria para la tabla numero del owner
        public int Id { get; set; }

        public User User { get; set; }

        //relacion entre tablas foreign keys (Location and owner)
        public Location Location { get; set; }

        //relacion entre Owner y Content
        public ICollection<Content> Contents { get; set; }
        //relacion entre Owner y comment
        public ICollection<Comment> Comments { get; set; }
    }
}
