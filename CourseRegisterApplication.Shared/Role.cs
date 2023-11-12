namespace CourseRegisterApplication.Shared
{
    public class Role
    {
        public int Id { get; set; }
        public RoleName RoleName { get; set; }
        public ICollection<User>? Users { get; }
    }
}
