using Microsoft.EntityFrameworkCore;

namespace lab31_brian.Models
{
    public class UserPostsContext : DbContext
    {
        public UserPostsContext (DbContextOptions<UserPostsContext> options)
            : base(options)
        {
        }

        public DbSet<UserPost> UserPost { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserPost>().HasKey(key => key.ApplicationUser.Id);
        }
    }
}
