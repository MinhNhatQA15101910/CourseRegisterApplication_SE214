namespace CourseRegisterApplication.Shared
{
    public class Branch
    {
        public int Id { get; set; }
        public string? BranchSpecificId { get; set; }
        public string? BranchName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
