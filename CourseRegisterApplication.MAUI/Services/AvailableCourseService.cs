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

        public async Task<bool> DeleteAvailaleCourse(int semesterId, int subjectId)
        {
            string apiUrl = $"{GlobalConfig.AVAILABLE_COURSE_BASE_URL}{semesterId}/{subjectId}";

            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<AvailableCourse> AddAvailableCourseAsync(AvailableCourse availableCourse)
        {
            string apiUrl = GlobalConfig.AVAILABLE_COURSE_BASE_URL;

            var json = JsonConvert.SerializeObject(availableCourse);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return availableCourse;
            }

            return null;
        }

        public async Task<bool> DeleteAvailableCoursesBySemesterIdAsync(int semesterId)
        {
            string apiUrl = $"{GlobalConfig.AVAILABLE_COURSE_BASE_URL}semesterId/{semesterId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}