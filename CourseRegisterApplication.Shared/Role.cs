namespace CourseRegisterApplication.Shared
{
    public class Role
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }

        public ICollection<User> Users { get; } = new List<User>();
    }
}
