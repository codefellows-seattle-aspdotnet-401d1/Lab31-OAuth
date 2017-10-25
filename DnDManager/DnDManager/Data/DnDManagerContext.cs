using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DnDManager.Models
{
    public class DnDManagerContext : DbContext
    {
        public DnDManagerContext (DbContextOptions<DnDManagerContext> options)
            : base(options)
        {
        }

        public DbSet<DnDManager.Models.Character> Character { get; set; }
    }
}
