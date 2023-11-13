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
        public async Task<User[]> GetAdminAccountantAccounts(string filter = "")
        {
            try
            {
                string apiUrl = $"{_baseUrl}?roleId={3}";

                if (!string.IsNullOrEmpty(filter))
                {
                    apiUrl += $"&filter={filter}";
                }

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(new Uri(apiUrl)).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var users = JsonConvert.DeserializeObject<User[]>(jsonResponse);

                        return users;
                    }
                }

                return null; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                string apiUrl = $"{_baseUrl}";

                using (HttpClient httpClient = new HttpClient())
                {
                    string userJson = JsonConvert.SerializeObject(user);
                    var content = new StringContent(userJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(new Uri(apiUrl), content).ConfigureAwait(false);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                string apiUrl = $"{_baseUrl}{userId}";

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
