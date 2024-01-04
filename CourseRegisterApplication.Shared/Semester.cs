namespace CourseRegisterApplication.Shared
{
    public class Semester
    {
        public int Id { get; set; }
        public SemesterName SemesterName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Year is required!"), Range(minimum: 2008, maximum: 2050, ErrorMessage = "Year not supported!")]
        public int Year { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required!"), Range(typeof(DateTime), "2008-01-01", "2050-12-31", ErrorMessage = "Date not supported!")]
        public DateTime StartRegistrationDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required!"), Range(typeof(DateTime), "2008-01-01", "2050-12-31", ErrorMessage = "Date not supported!")]
        public DateTime EndRegistrationDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Minimum credits is required!"), Range(minimum: 0, maximum: 40, ErrorMessage = "Out of range!")]
        public int MinimumCredits { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Maximum credits is required!"), Range(minimum: 0, maximum: 40, ErrorMessage = "Out of range!")]
        public int MaximumCredits { get; set; }
        public bool IsEnded { get; set; }
    }
}