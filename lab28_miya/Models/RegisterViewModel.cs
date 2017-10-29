using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab28_miya.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email
        {
            get; set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "Your password is too long. Must be 100 characters or less.", MinimumLength = 8)]
        //this will make the password hidden as soon it is typed
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password
        {
            get; set;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        //this allows us to compare password entries and will not allow the user to log in if passwords do not match
        [Compare("Password", ErrorMessage = "The passwords are not a match.")]
        public string ConfirmPassword
        {
            get; set;
        }

        [DataType(DataType.Date)]
        public DateTime StartDate
        {
            get; set;
        }

        [Required]
        public string FirstName
        {
            get; set;
        }
        [Required]
        public string LastName
        {
            get; set;
        }
    }
}
