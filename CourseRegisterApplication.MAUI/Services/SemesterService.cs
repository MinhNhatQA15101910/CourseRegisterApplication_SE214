using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly HttpClient _httpClient;

        public SemesterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Semester>> GetAllSemester()
        {
            string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var semesters = JsonConvert.DeserializeObject<List<Semester>>(jsonResponse);
                return semesters;
            }

            return null;
        }

        public async Task<Semester> GetSemesterByNameAndYear(string name, int year)
        {
            string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}/GetSemesterByNameAndYear?name={name}&year={year}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var semesterWithNameAndYear = JsonConvert.DeserializeObject<Semester>(jsonResponse);
                return semesterWithNameAndYear;
            }

            return null;
        }
    }
}
