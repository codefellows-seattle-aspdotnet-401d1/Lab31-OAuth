using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IdentityDay2.Models
{
    public class IdentityDay2Context : DbContext
    {
        public IdentityDay2Context (DbContextOptions<IdentityDay2Context> options)
            : base(options)
        {
        }

        public DbSet<IdentityDay2.Models.CMS> CMS { get; set; }
    }
}
