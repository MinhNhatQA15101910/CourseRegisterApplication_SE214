namespace CourseRegisterApplication.Shared
{
    public class User
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required!"), MinLength(3, ErrorMessage = "Username must be at least 3 characters!")]
        public string? Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!"), DataType(DataType.Password, ErrorMessage = "Password is not valid!"), MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
        public string? Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!"), DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid!")]
        public string? Email { get; set; }
        public Role Role { get; set; }
    }
}
