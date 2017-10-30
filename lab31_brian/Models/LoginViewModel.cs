using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lab31_brian.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
