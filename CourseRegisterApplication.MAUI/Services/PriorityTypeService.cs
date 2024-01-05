using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class PriorityTypeService : IPriorityTypeService
    {
        private readonly HttpClient _httpClient;

        public PriorityTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PriorityType>> GetAllPriorityType()
        {
            string apiUrl = $"{GlobalConfig.PRIORITY_TYPE_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var priorityTypeList = JsonConvert.DeserializeObject<List<PriorityType>>(jsonResponse);
                return priorityTypeList;
            }

            return null;
        }

        public Task<IEnumerable<PriorityType>> GetAllPriorityTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PriorityType>> GetPriorityTypesFromStudentIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}