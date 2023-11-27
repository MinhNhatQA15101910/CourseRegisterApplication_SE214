namespace CourseRegisterApplication.Shared
{
    public class District
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "District Name is required!"), DataType(DataType.Text, ErrorMessage = "Invalid District Name")]
        public string? DistrictName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Priority is required!")]
        public bool IsPriority { get; set; }

        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public Province? Province { get; set; } = null!;
    }
}
