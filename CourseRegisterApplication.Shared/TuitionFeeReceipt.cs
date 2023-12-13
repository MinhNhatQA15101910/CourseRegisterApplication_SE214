namespace CourseRegisterApplication.Shared
{
    public class TuitionFeeReceipt
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tuition fee is required!")]
        public string? TuitionFeeReceiptSpecificId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date is required!"), DataType(DataType.DateTime, ErrorMessage = "Wrong format!")]
        public DateTime CreatedDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Charge is required!"), Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Out of range!")]
        public double Charge { get; set; }
        public TuitionFeeReceiptState State { get; set; }

        [ForeignKey("CourseRegistrationForm")]
        public int CourseRegistrationFormId { get; set; }
        public CourseRegistrationForm? CourseRegistrationForm { get; set; } = null!;
    }
}