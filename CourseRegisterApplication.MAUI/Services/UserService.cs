using CourseRegisterApplication.MAUI.IServices;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CourseRegisterApplication.MAUI.Services
{
    public class UserService : IUserService
	{
		private readonly string _baseUrl = "https://localhost:7182/api/Users/";
        private readonly HttpClient _httpClient = new HttpClient();

        private List<User> users;

        public async Task<User> LoginUser(string username, string password)
		{
			string apiUrl = $"{_baseUrl}{username}/{password}";

			var response = await _httpClient.GetAsync(new Uri(apiUrl));
			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				var user = JsonConvert.DeserializeObject<User>(jsonResponse);

				return user;
			}
			else
			{
				return null;
			}
		}
        public async Task<List<User>> GetAdminAccountantAccounts()
        {
            try
            {
                // Base URL for admins and accountants
                string adminApiUrl = $"{_baseUrl}?role={0}";
                string accountantApiUrl = $"{_baseUrl}?role={1}"; 
                
                using (HttpClient httpClient = new HttpClient())
                {
                    var adminResponse = await httpClient.GetAsync(new Uri(adminApiUrl)).ConfigureAwait(false);
                    var accountantResponse = await httpClient.GetAsync(new Uri(accountantApiUrl)).ConfigureAwait(false);

                    // Check if both requests were successful
                    if (adminResponse.IsSuccessStatusCode && accountantResponse.IsSuccessStatusCode)
                    {
                        // Read the response content as strings
                        string adminJsonResponse = await adminResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        string accountantJsonResponse = await accountantResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        // Deserialize the JSON responses into arrays of User objects
                        var adminUsers = JsonConvert.DeserializeObject<List<User>>(adminJsonResponse);
                        var accountantUsers = JsonConvert.DeserializeObject<List<User>>(accountantJsonResponse);

                        // Combine the arrays and return the result
                        users = adminUsers.Concat(accountantUsers).ToList();
                        return adminUsers.Concat(accountantUsers).ToList();
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
        public async Task<List<User>> GetStudentAccounts()
        {
            try
            {
                string studentApiUrl = $"{_baseUrl}?role={2}";

                using (HttpClient httpClient = new HttpClient())
                {
                    var studentResponse = await httpClient.GetAsync(new Uri(studentApiUrl)).ConfigureAwait(false);

                    if (studentResponse.IsSuccessStatusCode)
                    {
                        string studentJsonResponse = await studentResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var studentUsers = JsonConvert.DeserializeObject<List<User>>(studentJsonResponse);

                        return studentUsers.ToList();
                    }
                }

                return null;
            } catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        // Filters for both AdminAccountant and Student
        public List<User> FilterBySearchBox(string filter)
        {
            return users.Where(u => u.Username.Contains(filter) || u.Email.Contains(filter)).ToList();
        }
        public List<User> FilterUserAZByUsername()
        {
            return users.OrderBy(u => u.Username).ToList(); 
        }
        public List<User> FilterUserZAByUsername()
        {
            return users.OrderByDescending(u => u.Username).ToList();
        }
        // Email just use for ADMIN and ACCOUNTANT accounts
        public List<User> FilterUserAZByEmail()
        {
            return users.OrderBy(u => u.Email).ToList();
        }
        public List<User> FilterUserZAByEmail()
        {
            return users.OrderByDescending(u => u.Email).ToList();
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
