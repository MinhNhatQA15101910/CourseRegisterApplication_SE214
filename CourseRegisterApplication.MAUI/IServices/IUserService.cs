using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
        Task<List<User>> GetAdminAccountantAccounts();
        Task<List<User>> GetStudentAccounts();
        List<User> FilterBySearchBox(string filter);
        List<User> FilterUserAZByUsername();
        List<User> FilterUserZAByUsername();
        List<User> FilterUserAZByEmail();
        List<User> FilterUserZAByEmail();
        Task<bool> CreateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
