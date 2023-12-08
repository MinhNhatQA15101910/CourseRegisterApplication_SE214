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

        public async Task<Branch> AddBranch(Branch branch)
        {
            string apiUrl = $"{GlobalConfig.BRANCH_BASE_URL}";

            var json = JsonConvert.SerializeObject(branch);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return branch;
            }

            return null;
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            string apiUrl = $"{GlobalConfig.BRANCH_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var branchList = JsonConvert.DeserializeObject<List<Branch>>(jsonResponse);
                return branchList;
            }

            return null;
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

        public async Task<bool> UpdateBranch(int branchId, Branch branch)
        {
            string apiUrl = $"{GlobalConfig.BRANCH_BASE_URL}{branchId}";

            var json = JsonConvert.SerializeObject(branch);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }
    }
}
