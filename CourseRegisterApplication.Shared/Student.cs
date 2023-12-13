namespace CourseRegisterApplication.Shared
{
    public class Student
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Student Id is required!"), DataType(DataType.Text, ErrorMessage = "Student Id is not valid!")]
        public string? StudentSpecificId { get; set; }
        public string? ImageUrl { get; set; } = "https://static.wixstatic.com/media/8027bc_6d79e9c44bae49de97c018a781738884~mv2.jpg/v1/fill/w_987,h_1096,al_c,q_90/file.jpg";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Student Full Name is required!"), DataType(DataType.Text, ErrorMessage = "Student Full Name is not valid!")]
        public string? FullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "No email no detail!"), DataType(DataType.Text, ErrorMessage = "Email is not valid!")]
        public string? Email { get; set; }
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