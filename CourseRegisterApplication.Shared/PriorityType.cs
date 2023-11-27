namespace CourseRegisterApplication.Shared
{
    public class PriorityType
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Priority Name is required!"), DataType(DataType.Text, ErrorMessage = "Priority Name is not valid!")]
        public string? PriorityName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tuition Discount Rate is required!"), Range(minimum: 0, maximum: 1, ErrorMessage = "Tuition Discount Rate must be between 0 and 1!")]
        public float TuitionDiscountRate { get; set; }

        public ICollection<StudentPriorityType>? Students { get; }
    }
}
