using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly HttpClient _httpClient;

        public SubjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Subject> GetSubjectById(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subject = JsonConvert.DeserializeObject<Subject>(jsonResponse);
                return subject;
            }

            return null;
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjects = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjects;
            }

            return null;
        }

        public async Task<Subject> CreateSubject(Subject subject)
        {
            string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return subject;
            }

            return null;
        }

        public async Task<bool> DeleteSubject(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateSubject(int subjectId, Subject subject)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";

            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsForFirstSemesterAsync()
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}firstSemester/";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsForSecondSemesterAsync()
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}secondSemester/";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsForSummerSemesterAsync()
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}summerSemester/";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsBySemesterIdAsync(int semesterId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}semesterId/{semesterId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsBySubjectTypeIdAsync(int subjectTypeId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}subjectTypeId/{subjectTypeId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjects = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjects;
            }

            return null;
        }
    }
}
