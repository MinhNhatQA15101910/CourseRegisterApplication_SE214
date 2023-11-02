namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserServices
    {
        Task<User> LoginUser(string username, string password);
    }
}
