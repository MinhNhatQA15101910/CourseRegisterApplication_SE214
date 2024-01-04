namespace CourseRegisterApplication.Shared
{
    public class Semester
    {
        public int Id { get; set; }
        public SemesterName SemesterName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Year is required!"), Range(minimum: 2008, maximum: 2050, ErrorMessage = "Year not supported!")]
        public int Year { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required!"), Range(typeof(DateTime), "2008-01-01", "2050-12-31", ErrorMessage = "Date not supported!")]
        [CustomValidation(typeof(Semester), "ValidateRegistrationDate")]
        public DateTime StartRegistrationDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required!"), Range(typeof(DateTime), "2008-01-01", "2050-12-31", ErrorMessage = "Date not supported!")]
        [CustomValidation(typeof(Semester), "ValidateRegistrationDate")]
        public DateTime EndRegistrationDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Minimum credits is required!"), Range(minimum: 0, maximum: 40, ErrorMessage = "Out of range!")]
        [CustomValidation(typeof(Semester), "ValidateCredits")]
        public int MinimumCredits { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Maximum credits is required!"), Range(minimum: 0, maximum: 40, ErrorMessage = "Out of range!")]
        [CustomValidation(typeof(Semester), "ValidateCredits")]
        public int MaximumCredits { get; set; }
        public bool IsEnded { get; set; }

        public static ValidationResult ValidateRegistrationDate(Semester semester)
        {
            if (semester.StartRegistrationDate > semester.EndRegistrationDate)
            {
                return new ValidationResult("Start date later than End date. Please re provide!");
            }

            return ValidationResult.Success ?? new ValidationResult("ValidationResult.Success is unexpectedly null.");
        }
        public static ValidationResult ValidateCredits(Semester semester)
        {
            if (semester.MinimumCredits > semester.MaximumCredits)
            {
                return new ValidationResult("MinimumCredits must be less than or equal to MaximumCredits.");
            }

            return ValidationResult.Success ?? new ValidationResult("ValidationResult.Success is unexpectedly null.");
        }
    }
}