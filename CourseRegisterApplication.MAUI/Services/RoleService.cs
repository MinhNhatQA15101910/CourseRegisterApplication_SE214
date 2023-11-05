using CourseRegisterApplication.MAUI.IServices;
using Newtonsoft.Json;

namespace CourseRegisterApplication.MAUI.Services
{
    public class RoleService : IRoleService
    {
        private readonly string _baseUrl = "https://localhost:7182/api/Roles/";
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<Role> GetRole(int id)
        {
            string apiUrl = $"{_baseUrl}{id}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<Role>(jsonResponse);
                return role;
            }
            else
            {
                return null;
            }
        }
    }
}
