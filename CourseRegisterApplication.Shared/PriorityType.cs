namespace CourseRegisterApplication.Shared
{
    public class PriorityType
    {
        public int Id { get; set; }
        public string? PriorityName { get; set; }
        public float TuitionDiscountRate { get; set; }
        public ICollection<StudentPriorityType>? Students { get; } 
    }
}
