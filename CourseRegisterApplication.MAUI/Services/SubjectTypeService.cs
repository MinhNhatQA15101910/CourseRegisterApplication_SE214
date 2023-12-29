using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SubjectTypeService : ISubjectTypeService
    {
        private readonly HttpClient _httpClient;

        public SubjectTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubjectType>> GetAllSubjectType()
        {
            string apiUrl = GlobalConfig.SUBJECT_TYPE_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var branchList = JsonConvert.DeserializeObject<List<SubjectType>>(jsonResponse);
                return branchList;
            }

            return null;
        }

        public async Task<SubjectType> GetSubjectTypeById(int subjectTypeId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_TYPE_BASE_URL}{subjectTypeId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SubjectType>(jsonResponse);
            }

            return null;
        }
    }
}
