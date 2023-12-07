namespace CourseRegisterApplication.Shared
{
    public class AvailableCourse
    {
        [ForeignKey("Semester")]
        public int SemesterId { get; set; }
        public Semester? Semester { get; set; } = null!;

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; } = null!;
    }
}
