using CourseRegisterApplication.MAUI.IServices;
using Newtonsoft.Json;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private readonly string _baseUrl = "https://localhost:7182/api/Users/";
        private readonly HttpClient _httpClient = new HttpClient();

		private readonly IRoleService _roleService = new RoleService();

		public async Task<User> LoginUser(string username, string password)
		{
			string apiUrl = $"{_baseUrl}{username}/{password}";

			var response = await _httpClient.GetAsync(new Uri(apiUrl));
			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<User>(jsonResponse);
				if (user != null)
				{
					user.Role = await _roleService.GetRole(user.RoleId);
				}
				return user;
			}
			else
			{
				return null;
			}
		}
	}
}
