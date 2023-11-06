namespace CourseRegisterApplication.Shared
{
    public class Province
    {
        public int Id { get; set; }
        public string? ProvinceName { get; set; }

        public ICollection<District> Districts { get; } = new List<District>();
    }
}
