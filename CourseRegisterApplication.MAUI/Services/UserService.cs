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

        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> FilterBySearchBox(string filter)
        {
            throw new NotImplementedException();
        }

        public List<User> FilterUserAZByEmail()
        {
            throw new NotImplementedException();
        }

        public List<User> FilterUserAZByUsername()
        {
            throw new NotImplementedException();
        }

        public List<User> FilterUserZAByEmail()
        {
            throw new NotImplementedException();
        }

        public List<User> FilterUserZAByUsername()
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAdminAccountantAccounts()
        {
            var response = await _httpClient.GetAsync(new Uri(_baseUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<List<User>>(jsonResponse);
                return userList.Where(u => u.Role == Role.Admin || u.Role == Role.Accountant).ToList();
            }

            return null;
        }

        public Task<List<User>> GetStudentAccounts()
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
				
			return null;
		}
	}
}
