using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityDay2.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityDay2.Data
{
    public class AppDbContext : IdentityDbContext<CrewMember>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<IdentityDay2.Models.CrewMember> CrewMember { get; set; }
    }
}
