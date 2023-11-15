namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IUserService
    {
        Task<User> LoginUser(string username, string password);
		Task<User> ChangePassword(string newPassword);

		Task<User> AddUpdateAccount(string username, string password, string email);
	}
}
