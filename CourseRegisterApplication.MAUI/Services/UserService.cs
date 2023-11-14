using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private readonly string _baseUrl = "https://localhost:7182/api/Users/";
        private readonly HttpClient _httpClient = new HttpClient();

        public Task<User> ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<User> LoginUser(string username, string password)
		{
			string apiUrl = $"{_baseUrl}{username}/{password}";

			var response = await _httpClient.GetAsync(new Uri(apiUrl));
			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<User>(jsonResponse);
			}
			else
			{
				return null;
			}
		}
	}
}
