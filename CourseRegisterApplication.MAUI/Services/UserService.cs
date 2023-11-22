using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private readonly string _baseUrl = "https://localhost:7182/api/Users/";
		private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> AddUser(User user)
        {
            string apiUrl = $"{_baseUrl}";

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
				return user;
            }

            return null;
        }

        public async Task<bool> ChangePassword(int id, User user)
        {
			string apiUrl = $"{_baseUrl}{id}";

			var json = JsonConvert.SerializeObject(user);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
				
			return false;
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
				
			return null;
		}
	}
}
