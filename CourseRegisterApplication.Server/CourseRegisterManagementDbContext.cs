using CourseRegisterApplication.Shared;
using Microsoft.EntityFrameworkCore;

namespace CourseRegisterApplication.Server
{
    public class CourseRegisterManagementDbContext : DbContext
    {
        public CourseRegisterManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role
            modelBuilder.Entity<Role>(entity => entity.HasIndex(e => e.RoleName).IsUnique());

            // User
            modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Username).IsUnique());
            modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        }
    }
}
