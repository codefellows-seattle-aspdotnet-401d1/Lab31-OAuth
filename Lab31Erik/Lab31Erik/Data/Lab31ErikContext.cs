using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab31Erik.Models
{
    public class Lab31ErikContext : DbContext
    {
        public Lab31ErikContext (DbContextOptions<Lab31ErikContext> options)
            : base(options)
        {
        }

        public DbSet<Lab31Erik.Models.Product> Product { get; set; }
    }
}
