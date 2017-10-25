using Microsoft.AspNetCore.Identity;
using System;

namespace DnDManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthday { get; set; }
    }
}
