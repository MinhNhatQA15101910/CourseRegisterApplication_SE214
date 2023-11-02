using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterApplication.Shared
{
    public class StudentPriorityType
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [ForeignKey("PriorityType")]
        public int PriorityTypeId { get; set; }
        public PriorityType PriorityType { get; set; } = null!;
    }
}
