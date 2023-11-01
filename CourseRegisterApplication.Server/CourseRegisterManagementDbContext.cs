namespace CourseRegisterApplication.Server;

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

        // Generate data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Roles
        var roles = new Role[]
        {
            new Role { Id = 1, RoleName = "Admin" },
            new Role { Id = 2, RoleName = "Accountant" },
            new Role { Id = 3, RoleName = "Student" }
        };

        // Users
        var users = new User[]
        {
            new User
            {
                Id = 1,
                Username = "admin1",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "admin1.uit@gmail.com",
                RoleId = 1
            },
            new User
            {
                Id = 2,
                Username = "teacher1",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "teacher1.uit@gmail.com",
                RoleId = 2
            },
            new User
            {
                Id = 3,
                Username = "SV21522415",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "21522415@gm.uit.edu.vn",
                RoleId = 3
            },
            new User
            {
                Id = 4,
                Username = "SV21522217",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "21522217@gm.uit.edu.vn",
                RoleId = 3
            },
            new User
            {
                Id = 5,
                Username = "SV21522819",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "21522819@gm.uit.edu.vn",
                RoleId = 3
            },
            new User
            {
                Id = 6,
                Username = "SV21521682",
                Password = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE",
                Email = "21521682@gm.uit.edu.vn",
                RoleId = 3
            }
        };

        modelBuilder.Entity<Role>().HasData(roles);
        modelBuilder.Entity<User>().HasData(users);
    }
}
