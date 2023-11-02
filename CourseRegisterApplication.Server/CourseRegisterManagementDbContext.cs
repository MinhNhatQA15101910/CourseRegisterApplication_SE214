namespace CourseRegisterApplication.Server;

public class CourseRegisterManagementDbContext : DbContext
{
    public CourseRegisterManagementDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Branch> Branches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        InitializeUniques(modelBuilder);

        // Generate data
        SeedData.Initialize(modelBuilder);
    }

    private void InitializeUniques(ModelBuilder modelBuilder)
    {
        // Role
        modelBuilder.Entity<Role>(entity => entity.HasIndex(e => e.RoleName).IsUnique());

        // User
        modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Username).IsUnique());
        modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());

        // Province
        modelBuilder.Entity<Province>(entity => entity.HasIndex(e => e.ProvinceName).IsUnique());

        // Department
        modelBuilder.Entity<Department>(entity => entity.HasIndex(e => e.DepartmentNameId).IsUnique());
        modelBuilder.Entity<Department>(entity => entity.HasIndex(e => e.DepartmentName).IsUnique());

        // Branch
        modelBuilder.Entity<Branch>(entity => entity.HasIndex(e => e.BranchNameId).IsUnique());
        modelBuilder.Entity<Branch>(entity => entity.HasIndex(e => e.BranchName).IsUnique());
    }
}
