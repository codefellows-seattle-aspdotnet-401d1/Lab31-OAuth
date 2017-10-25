using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDay2.Models
{
    public class CrewMember : IdentityUser
    {
        public string Rank { get; set; }
        public Channel Department { get; set; }
    }
}
