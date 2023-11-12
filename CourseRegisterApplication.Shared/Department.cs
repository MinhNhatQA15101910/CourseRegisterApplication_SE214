namespace CourseRegisterApplication.Shared
{
    public class Department
    {
        public int Id { get; set; }
        public string? DepartmentSpecificId { get; set; }
        public string? DepartmentName { get; set; }
        public ICollection<Branch>? Branches { get; } 
    }
}
