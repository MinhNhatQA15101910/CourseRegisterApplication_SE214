using Microsoft.Build.ObjectModelRemoting;

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
    public DbSet<SubjectType> SubjectTypes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Curriculum> Curriculums { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<AvailableCourse> AvailableCourses { get; set; }
    public DbSet<CourseRegistrationForm> CourseRegistrationForms { get; set; }
    public DbSet<CourseRegistrationDetail> CourseRegistrationDetails { get; set; }
    public DbSet<TuitionFeeReceipt> TuitionFeeReceipts { get; set; }

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

        // Subject Type 
        modelBuilder.Entity<SubjectType>().HasIndex(st => st.Name).IsUnique();

        // Subject
        modelBuilder.Entity<Subject>().HasIndex(s => s.SubjectSpecificId).IsUnique();

        // Curriculum
        modelBuilder.Entity<Curriculum>().HasKey(c => new { c.BranchId, c.SubjectId });

        // Semester
        modelBuilder.Entity<Semester>().HasIndex(s => new { s.Year, s.SemesterName }).IsUnique();

        // Available Course
        modelBuilder.Entity<AvailableCourse>().HasKey(ac => new { ac.SemesterId, ac.SubjectId });

        // Course registration form
        modelBuilder.Entity<CourseRegistrationForm>().HasIndex(crf => crf.CourseRegistrationFormSpecificId).IsUnique();
        modelBuilder.Entity<CourseRegistrationForm>().HasIndex(crf => new { crf.SemesterId, crf.StudentId }).IsUnique();

        // Course registration detail
        modelBuilder.Entity<CourseRegistrationDetail>().HasKey(crd => new { crd.CourseRegistrationFormId, crd.SubjectId });

        // Tuition fee receipt
        modelBuilder.Entity<TuitionFeeReceipt>().HasIndex(tfr => tfr.TuitionFeeReceiptSpecificId).IsUnique();
    }
}