using System.ComponentModel.DataAnnotations;

namespace DnDManager.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public string Race { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        [Required]
        public bool IsAlive { get; set; }
        public string PhysicalDescription { get; set; }
    }
}
