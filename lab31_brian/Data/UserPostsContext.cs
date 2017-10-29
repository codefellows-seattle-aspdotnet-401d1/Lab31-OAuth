using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace lab31_brian.Models
{
    public class UserPostsContext : DbContext
    {
        public UserPostsContext (DbContextOptions<UserPostsContext> options)
            : base(options)
        {
        }

        public DbSet<lab31_brian.Models.UserPost> UserPost { get; set; }
    }
}
