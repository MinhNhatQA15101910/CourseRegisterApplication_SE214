namespace CourseRegisterApplication.Shared
{
    public class Department
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Department Id is required!"), DataType(DataType.Text, ErrorMessage = "Department Id is invalid!")]
        public string? DepartmentSpecificId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Department Name is required!"), DataType(DataType.Text, ErrorMessage = "Department Name is invalid!")]
        public string? DepartmentName { get; set; }

        public ICollection<Branch>? Branches { get; } = null!;
    }
}
