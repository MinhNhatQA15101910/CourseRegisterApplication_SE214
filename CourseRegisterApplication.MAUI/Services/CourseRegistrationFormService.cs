using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class CourseRegistrationFormService : ICourseRegistrationFormService
    {
        private readonly HttpClient _httpClient;

        public CourseRegistrationFormService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseRegistrationForm>> GetAllCourseRegistrationForm()
        {
            string apiUrl = GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationForms = JsonConvert.DeserializeObject<List<CourseRegistrationForm>>(jsonResponse);
                return courseRegistrationForms;
            }

            return null;
        }

        public async Task<CourseRegistrationForm> GetCourseRegistrationFormById(int id)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL}{id}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationForm = JsonConvert.DeserializeObject<CourseRegistrationForm>(jsonResponse);
                return courseRegistrationForm;
            }

            return null;
        }

        public async Task<CourseRegistrationForm> CreateCourseRegistrationForm(CourseRegistrationForm courseRegistrationForm)
        {
            string apiUrl = GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL;

            var json = JsonConvert.SerializeObject(courseRegistrationForm);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return courseRegistrationForm;
            }

            return null;
        }

        public async Task<bool> UpdateCourseRegistrationForm(int id, CourseRegistrationForm courseRegistrationForm)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL}{id}";

            var json = JsonConvert.SerializeObject(courseRegistrationForm);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<CourseRegistrationForm>> GetCourseRegistrationFormByStudentId(int studentId)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationForms = JsonConvert.DeserializeObject<List<CourseRegistrationForm>>(jsonResponse);
                return courseRegistrationForms.Where(crf => crf.StudentId == studentId).ToList();
            }

            return null;
        }
        public async Task<CourseRegistrationForm> GetCourseRegistrationFormByStudentIdAndSemesterId(int studentId, int semesterId)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_FORM_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationForms = JsonConvert.DeserializeObject<List<CourseRegistrationForm>>(jsonResponse);
                return courseRegistrationForms.Where(crf => crf.StudentId == studentId && crf.SemesterId == semesterId).FirstOrDefault();
            }

            return null;
        }
    }
}