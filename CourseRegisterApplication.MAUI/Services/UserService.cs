using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserServices
	{
		public Task<User> LoginUser(string username, string password)
		{
			throw new NotImplementedException();
		}
	}
}
