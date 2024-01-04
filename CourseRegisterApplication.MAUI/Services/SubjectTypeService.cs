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

        public async Task<SubjectType> GetAllSubjectType(int subjectTypeId)
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

        public async Task<SubjectType> CreateSubjectType(SubjectType subjectType)
        {
            string apiUrl = GlobalConfig.SUBJECT_TYPE_BASE_URL;

            var json = JsonConvert.SerializeObject(subjectType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return subjectType;
            }

            return null;
        }
        public async Task<bool> UpdateSubjectType(int id, SubjectType subjectType)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_TYPE_BASE_URL}{id}";

            var json = JsonConvert.SerializeObject(subjectType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteSubjectType(int id)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_TYPE_BASE_URL}{id}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
