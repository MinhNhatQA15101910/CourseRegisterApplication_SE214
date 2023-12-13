namespace CourseRegisterApplication.Shared
{
    public class CourseRegistrationDetail
    {
        [ForeignKey("CourseRegistrationForm")]
        public int CourseRegistrationFormId { get; set; }
        public CourseRegistrationForm? CourseRegistrationForm { get; set; } = null!;
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; } = null!;
    }
}
