using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace lab28_miya.Models
{
    public class lab28_miyaContext : DbContext
    {
        public lab28_miyaContext (DbContextOptions<lab28_miyaContext> options)
            : base(options)
        {
        }

        public DbSet<lab28_miya.Models.CPS> CPS { get; set; }
    }
}
