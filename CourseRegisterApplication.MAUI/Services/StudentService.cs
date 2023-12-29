using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
            

        public Task<Student> AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetStudents()
        {
            throw new NotImplementedException();
        }

        public Task<Student> AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetStudents()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> GetStudentsByBranchId(int branchId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}branch/{branchId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public async Task<List<Student>> GetStudentsByDistrictId(int districtId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}district/{districtId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }
    }
}
