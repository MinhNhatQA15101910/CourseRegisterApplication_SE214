namespace CourseRegisterApplication.MAUI.IServices;

public interface IUserService
{
    Task<User> LoginUser(string username, string password);
    Task<bool> ChangePassword(User user, string newPassword);
    Task<User> AddUser(User user);
    Task<List<User>> GetManagerUsers();
    Task<List<User>> GetStudentUsers();
    Task<List<User>> GetAllUsers();
    Task<bool> DeleteUser(int id);
    Task<bool> UpdateRole(User user, Role role);
}
