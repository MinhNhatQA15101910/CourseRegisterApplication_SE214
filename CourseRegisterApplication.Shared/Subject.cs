namespace CourseRegisterApplication.Shared
{
    public class Subject
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Subject specific ID is required!")]
        public string? SubjectSpecificId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Subject name is required!")]
        public string? Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Number of credtis is required!"), Range(minimum: 0, maximum: 200, ErrorMessage = "Out of range!")]
        public int NumberOfCredits { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Total lessons is required!"), Range(minimum: 0, maximum: 200, ErrorMessage = "Out of range!")]
        public int TotalLessons { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Total charge is required!"), Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Out of range!")]
        public double TotalCharge { get; set; }

        [ForeignKey("SubjectType")]
        public int SubjectTypeId { get; set; }
        public SubjectType? SubjectType { get; set; } = null!;

        public ICollection<Curriculum>? Curriculums { get; } = null!;
    }
}
