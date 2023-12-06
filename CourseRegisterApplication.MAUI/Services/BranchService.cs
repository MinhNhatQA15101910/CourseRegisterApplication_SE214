using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class BranchService : IBranchService
    {
        private readonly HttpClient _httpClient;

        public BranchService(IServiceProvider serviceProvider)
        {
            _httpClient = serviceProvider.GetService<HttpClient>();
        }

        public async Task<List<Branch>> GetBranchesByDepartmentId(int departmentId)
        {
            string apiUrl = $"{GlobalConfig.BRANCH_BASE_URL}{departmentId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var branchList = JsonConvert.DeserializeObject<List<Branch>>(jsonResponse);
                return branchList;
            }

            return null;
        }
    }
}
