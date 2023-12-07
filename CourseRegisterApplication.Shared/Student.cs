namespace CourseRegisterApplication.Shared
{
	public class Student
	{
		public int Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Student Id is required!"), DataType(DataType.Text, ErrorMessage = "Student Id is not valid!")]
		public string? StudentSpecificId { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Student Full Name is required"), DataType(DataType.Text, ErrorMessage = "Student Full Name is not valid!")]
		public string? FullName { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Date of Birth is required!"), DataType(DataType.DateTime, ErrorMessage = "Date of Birth is not valid!")]
		public DateTime DateOfBirth { get; set; }
		public Gender Gender { get; set; }

		[ForeignKey("District")]
		public int DistrictId { get; set; }
		public District? District { get; set; } = null!;

		[ForeignKey("Branch")]
		public int BranchId { get; set; }
		public Branch? Branch { get; set; } = null!;

		public ICollection<StudentPriorityType>? PriorityTypes { get; } = null!;
	}
}
