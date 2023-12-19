namespace CourseRegisterApplication.Shared
{
    public class SubjectType
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Subject type name is required!"), DataType(DataType.Text, ErrorMessage = "Subject type name is invalid.")]
        public string? Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Number of lessons is required!"), Range(minimum: 0, maximum: 200, ErrorMessage = "Out of range!")]
        public int NumberOfLessons { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Number of lessons is required!"), Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Out of range!")]
        public double LessonsCharge { get; set; }

        public ICollection<Subject>? Subjects { get; } = null!;
    }
}