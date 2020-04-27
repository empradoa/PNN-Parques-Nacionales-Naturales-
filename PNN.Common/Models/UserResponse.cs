using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{
    public class UserResponse
    { 
        public String Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public ICollection<ContentResponse> Contents { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }
}
