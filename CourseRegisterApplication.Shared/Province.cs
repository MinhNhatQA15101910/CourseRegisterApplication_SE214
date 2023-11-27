namespace CourseRegisterApplication.Shared
{
    public class Province
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Province Name is required!"), DataType(DataType.Text, ErrorMessage = "Province Name is not valid!")]
        public string? ProvinceName { get; set; }

        public ICollection<District>? Districts { get; } = null!;
    }
}
