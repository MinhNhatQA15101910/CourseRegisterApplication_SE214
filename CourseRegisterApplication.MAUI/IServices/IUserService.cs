namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
        Task<bool> ChangePassword(int id, User user);
        Task<User> AddUser(User user);
    }
}
