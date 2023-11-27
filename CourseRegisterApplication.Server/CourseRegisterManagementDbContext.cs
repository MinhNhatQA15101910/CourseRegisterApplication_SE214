namespace CourseRegisterApplication.Server;

public class CourseRegisterManagementDbContext : DbContext
{
    public CourseRegisterManagementDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<PriorityType> PriorityTypes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentPriorityType> StudentPriorityTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        InitializeUniques(modelBuilder);

        // Generate data
        SeedData.Initialize(modelBuilder);
    }

    private void InitializeUniques(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Username).IsUnique());
        modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());

        // Province
        modelBuilder.Entity<Province>(entity => entity.HasIndex(e => e.ProvinceName).IsUnique());

        // District
        modelBuilder.Entity<District>(entity => entity.HasIndex(e => new { e.DistrictName, e.ProvinceId }).IsUnique());

        // Department
        modelBuilder.Entity<Department>(entity => entity.HasIndex(e => e.DepartmentSpecificId).IsUnique());
        modelBuilder.Entity<Department>(entity => entity.HasIndex(e => e.DepartmentName).IsUnique());

        // Branch
        modelBuilder.Entity<Branch>(entity => entity.HasIndex(e => e.BranchSpecificId).IsUnique());
        modelBuilder.Entity<Branch>(entity => entity.HasIndex(e => e.BranchName).IsUnique());

        // Priority Type
        modelBuilder.Entity<PriorityType>(entity => entity.HasIndex(e => e.PriorityName).IsUnique());

        // Student
        modelBuilder.Entity<Student>(entity => entity.HasIndex(e => e.StudentSpecificId).IsUnique());

        // Student Priority Type
        modelBuilder.Entity<StudentPriorityType>().HasKey(spt => new { spt.StudentId, spt.PriorityTypeId });
    }
}
