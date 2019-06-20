using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class RegisterPostDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        //[EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        //public string UserRole { get; set; }
    }
}
