namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
        Task<User[]> GetAdminAccountantAccounts(string filter = "");
        Task<bool> CreateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
