namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
        Task<bool> ChangePassword(int id, User user);
        Task<User> AddUser(User user);
        Task<List<User>> GetAdminAccountantAccounts();
        Task<List<User>> GetStudentAccounts();
        List<User> FilterBySearchBox(string filter);
        List<User> FilterUserAZByUsername();
        List<User> FilterUserZAByUsername();
        List<User> FilterUserAZByEmail();
        List<User> FilterUserZAByEmail();
        Task<bool> DeleteUser(int id);
    }
}
