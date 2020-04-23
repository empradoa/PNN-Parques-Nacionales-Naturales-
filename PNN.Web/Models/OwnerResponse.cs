using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PNN.Web.Models
{
    public class OwnerResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
