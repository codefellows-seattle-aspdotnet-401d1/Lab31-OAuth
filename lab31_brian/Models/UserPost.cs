using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab31_brian.Models
{
    public class UserPost
    {
        [Key]
        public int ID { get; set; }

        public string Post { get; set; }

        public string Image { get; set; }

        public bool Published { get; set; }



        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
