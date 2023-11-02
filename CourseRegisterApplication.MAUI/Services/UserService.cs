using CourseRegisterApplication.MAUI.IServices;
using Newtonsoft.Json;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private static readonly HttpClient _httpClient = new HttpClient();

		public async Task<User> LoginUser(string username, string password)
		{
			string apiUrl = $"http://localhost:7182/api/{username}/{password}";
			HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<User>(content);
				return user;
			}
			else
			{
				return null;
			}
		}
	}
}
