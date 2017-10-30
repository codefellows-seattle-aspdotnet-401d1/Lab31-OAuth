using System.ComponentModel.DataAnnotations;

namespace lab31_brian.Models
{
    public class ExternalLoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
