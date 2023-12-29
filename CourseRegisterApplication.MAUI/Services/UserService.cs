using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> AddUser(User user)
        {
            string apiUrl = GlobalConfig.USER_BASE_URL;

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
				return user;
            }

            return null;
        }

        public async Task<bool> ChangePassword(User user, string newPassword)
        {
            user.Password = newPassword;

            string apiUrl = $"{GlobalConfig.USER_BASE_URL}{user.Id}";

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(int id)
        {
            string apiUrl = $"{GlobalConfig.USER_BASE_URL}{id}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<User>> GetManagerUsers()
        {
            var response = await _httpClient.GetAsync(new Uri(GlobalConfig.USER_BASE_URL));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<List<User>>(jsonResponse);
                return userList
                    .Where(u => u.Role == Role.Admin || u.Role == Role.Accountant)
                    .ToList();
            }

            return null;
        }

        public async Task<List<User>> GetStudentUsers()
        {
            var response = await _httpClient.GetAsync(new Uri(GlobalConfig.USER_BASE_URL));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<List<User>>(jsonResponse);
                return userList
                    .Where(u => u.Role == Role.Student)
                    .ToList();
            }

            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync(new Uri(GlobalConfig.USER_BASE_URL));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<List<User>>(jsonResponse);
                return userList.ToList();
            }

            return null;
        }

        public async Task<User> LoginUser(string username, string password)
	    {
			string apiUrl = $"{GlobalConfig.USER_BASE_URL}{username}/{password}";

			var response = await _httpClient.GetAsync(new Uri(apiUrl));
			if (response.IsSuccessStatusCode)
		    {
				string jsonResponse = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<User>(jsonResponse);
			}
				
			return null;
		}

        public async Task<bool> UpdateRole(User user, Role role)
        {
            user.Role = role;

            string apiUrl = $"{GlobalConfig.USER_BASE_URL}{user.Id}";

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            string apiUrl = $"{GlobalConfig.USER_BASE_URL}username/{username}";

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
