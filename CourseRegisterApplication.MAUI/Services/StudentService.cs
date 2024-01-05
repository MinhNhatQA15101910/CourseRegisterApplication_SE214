using CourseRegisterApplication.MAUI.IServices;

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

        public Task<Student> AddStudent(Student student, List<PriorityType> priorityTypes)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> GetAllStudents()
        {
            string apiUrl = GlobalConfig.STUDENT_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public Task<List<Student>> GetFullInformationOfAllStudents()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetFullInformationOfStudentBySpecificId(string studentSpecificId)
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetStudentBySpecificId(string studentSpecificId)
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

        public Task<bool> UpdateImageUrl(Student student, string newImageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudent(int studentId, Student student, List<PriorityType> priorityTypes)
        {
            throw new NotImplementedException();
        }
    }
}
