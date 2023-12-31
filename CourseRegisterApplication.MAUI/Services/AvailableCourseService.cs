using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class AvailableCourseService : IAvailableCourseService
    {
        private readonly HttpClient _httpClient;

        public AvailableCourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AvailableCourse>> GetAllAvailableCourse()
        {
            string apiUrl = $"{GlobalConfig.AVAILABLE_COURSE_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var availableCourse = JsonConvert.DeserializeObject<List<AvailableCourse>>(jsonResponse);
                return availableCourse;
            }

            return null;
        }
        public async Task<List<AvailableCourse>> GetAvailableCourseBySemesterId(int semesterId)
        {
            string apiUrl = $"{GlobalConfig.AVAILABLE_COURSE_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var availableCourse = JsonConvert.DeserializeObject<List<AvailableCourse>>(jsonResponse);
                return availableCourse.Where(ac => ac.SemesterId == semesterId).ToList();
            }

            return null;
        }

        public async Task<List<AvailableCourse>> GetAvailableCourseBySubjectId(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.AVAILABLE_COURSE_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var availableCourse = JsonConvert.DeserializeObject<List<AvailableCourse>>(jsonResponse);
                return availableCourse.Where(ac => ac.SubjectId == subjectId).ToList();
            }

            return null;
        }
    }
}
