using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lab31_brian.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [StringLength(40)]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Birthday")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Confirm Email")]
        [DataType(DataType.EmailAddress)]
        [Compare("Email", ErrorMessage = "Your Email doesn't not match")]
        public string ConfirmEmail { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password, ErrorMessage = "Your password doesn't meet our requirements")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
