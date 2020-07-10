using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PNN.Common.Models
{
    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [Required]
        public string CellPhone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
