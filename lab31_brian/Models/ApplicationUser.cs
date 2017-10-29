using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace lab31_brian.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}
