using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;

        public DepartmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Department> AddDepartment(Department department)
        {
            string apiUrl = $"{GlobalConfig.DEPARTMENT_BASE_URL}";

            var json = JsonConvert.SerializeObject(department);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return department;
            }

            return null;
        }

        public async Task<bool> DeleteDepartment(int departmentId)
        {
            string apiUrl = $"{GlobalConfig.DEPARTMENT_BASE_URL}{departmentId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            var response = await _httpClient.GetAsync(new Uri(GlobalConfig.DEPARTMENT_BASE_URL));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var departmentList = JsonConvert.DeserializeObject<List<Department>>(jsonResponse);
                return departmentList.ToList();
            }

            return null;
        }
    }
}
