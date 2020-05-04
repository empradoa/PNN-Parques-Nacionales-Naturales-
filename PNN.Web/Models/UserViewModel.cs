using System.Collections.Generic;
using PNN.Web.Data.Entities;
namespace PNN.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public ICollection<Content> Contents { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
