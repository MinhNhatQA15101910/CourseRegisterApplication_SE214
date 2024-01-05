using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly HttpClient _httpClient;
        private readonly IAvailableCourseService _availableCourseService;

        public SemesterService(HttpClient httpClient, IAvailableCourseService availableCourseService)
        {
            _httpClient = httpClient;
            _availableCourseService = availableCourseService;
        }

        public async Task<Semester> AddSemesterAsync(Semester semester, List<int> subjectIds)
        {
            string apiUrl = GlobalConfig.SEMESTER_BASE_URL;

            var json = JsonConvert.SerializeObject(semester);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                Semester createdSemester = await GetCurrentSemesterAsync();
                foreach (int subjectId in subjectIds)
                {
                    AvailableCourse availableCourse = new()
                    {
                        SemesterId = createdSemester.Id,
                        SubjectId = subjectId
                    };

                    AvailableCourse result = await _availableCourseService.AddAvailableCourseAsync(availableCourse);
                    if (result == null)
                    {
                        return null;
                    }
                }

                return semester;
            }

            return null;
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

        public async Task<Semester> GetCurrentSemesterAsync()
        {
            string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}current/";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var currentSemester = JsonConvert.DeserializeObject<Semester>(jsonResponse);
                return currentSemester;
            }

            return null;
        }

        public async Task<Semester> GetSemesterById(int semesterId)
        {
            string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var semesters = JsonConvert.DeserializeObject<List<Semester>>(jsonResponse);
                return semesters.Find(s => s.Id == semesterId);
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

        public async Task<bool> UpdateSemesterAsync(int semesterId, Semester semester)
        {
            string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}{semesterId}";

            var json = JsonConvert.SerializeObject(semester);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateSemesterAsync(int semesterId, Semester semester, List<int> subjectIds)
        {
            bool result = await UpdateSemesterAsync(semesterId, semester);
            if (result)
            {
                // Delete old subjects
                result = await _availableCourseService.DeleteAvailableCoursesBySemesterIdAsync(semesterId);
                if (!result)
                {
                    return false;
                }

                // Add new subjects
                Semester createdSemester = await GetCurrentSemesterAsync();
                foreach (int subjectId in subjectIds)
                {
                    AvailableCourse availableCourse = new()
                    {
                        SemesterId = createdSemester.Id,
                        SubjectId = subjectId
                    };

                    AvailableCourse result2 = await _availableCourseService.AddAvailableCourseAsync(availableCourse);
                    if (result2 == null)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
