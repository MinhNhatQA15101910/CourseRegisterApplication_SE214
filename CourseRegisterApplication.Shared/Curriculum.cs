namespace CourseRegisterApplication.Shared
{
    public class Curriculum
    {
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; } = null!;

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; } = null!;

        public int Semester {  get; set; }
    }
}
