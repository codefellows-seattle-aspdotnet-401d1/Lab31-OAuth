using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab28_miya.Models
{
    public class CPS
    {
        public int ID
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        [Display(Name = "Years in Service")]
        public int YearsInService
        {
            get; set;
        }
        public string Region
        {
            get; set;
        }
        [Display(Name = "Compassionate")]
        public bool HasCompassion
        {
            get; set;
        }
        public string Zodiac
        {
            get; set;
        }
    }
}
