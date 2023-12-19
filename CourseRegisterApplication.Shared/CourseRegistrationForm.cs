namespace CourseRegisterApplication.Shared
{
    public class CourseRegistrationForm
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required!"), DataType(DataType.Text, ErrorMessage = "Id is not valid!")]
        public string? CourseRegistrationFormSpecificId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date is required!"), DataType(DataType.DateTime, ErrorMessage = "Date time is not valid!")]
        public DateTime CreatedDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Charge is required!"), Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Out of range!")]
        public double Charge { get; set; }
        public CourseRegistrationFormState State { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student? Student { get; set; } = null!;

        [ForeignKey("Semester")]
        public int SemesterId { get; set; }
        public Semester? Semester { get; set; } = null!;
    }
}