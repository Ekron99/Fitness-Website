using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class RegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [MinLength(8)]
        public string password { get; set; }
        [MinLength(8)]
        public string confirmPassword { get; set; }
    }
}