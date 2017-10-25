using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace lab28_miya.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Circumstances
        {
            get; set;
        }
        [Display(Name ="First Name")]
        public string FirstName
        {
            get; set;
        }
        [Display(Name ="Last Name")]
        public string LastName
        {
            get; set;
        }
        [DataType(DataType.Date)]
        public DateTime StartDate
        {
            get; set;
        }

        //internal void AddIdentity(ClaimsIdentity userIdentity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
