namespace CourseRegisterApplication.Shared

{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
