using lab28_miya.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lab28_miya.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //this holds all tables related to identity and user authentication
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
