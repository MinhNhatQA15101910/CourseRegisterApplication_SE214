namespace CourseRegisterApplication.Shared
{
    public class Branch
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch Id is required!"), DataType(DataType.Text, ErrorMessage = "Invalid Branch Id!")]
        public string? BranchSpecificId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Branch Name is required!"), DataType(DataType.Text, ErrorMessage = "Invalid Branch Name!")]
        public string? BranchName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;

        public ICollection<Curriculum>? Curriculums { get; } = null!;
    }
}
