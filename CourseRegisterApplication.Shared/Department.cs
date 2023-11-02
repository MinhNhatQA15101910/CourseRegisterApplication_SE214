namespace CourseRegisterApplication.Shared
{
    public class Department
    {
        public int Id { get; set; }
        public string? DepartmentNameId { get; set; }
        public string? DepartmentName { get; set; }

        public ICollection<Branch> Branches { get; } = new List<Branch>();
    }
}
