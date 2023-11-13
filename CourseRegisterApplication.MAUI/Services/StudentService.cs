using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;
using Newtonsoft.Json;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
        private readonly string _baseUrl = "https://localhost:7182/api/Students/";
        private readonly HttpClient _httpClient = new HttpClient();
        private List<Student> _students;

        public async Task<List<Student>> GetAllStudent()
        {
            try
            {
                string studentsApiUrl = $"{_baseUrl}";

                using (HttpClient httpClient = new HttpClient())
                {
                    var studentsResponse = await httpClient.GetAsync(new Uri(studentsApiUrl)).ConfigureAwait(false);

                    if (studentsResponse.IsSuccessStatusCode)
                    {
                        string studentJsonResponse = await studentsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var students = JsonConvert.DeserializeObject<List<Student>>(studentJsonResponse);

                        _students = students.ToList();
                        return _students;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public List<Student> FilterBySearchBox(string filter)
        {
            return _students.Where(u => u.StudentSpecificId.Contains(filter)).ToList();
        }
        public List<Student> FilterUserAZByStudentSpecificId()
        {
            return _students.OrderBy(u => u.StudentSpecificId).ToList();
        }
        public List<Student> FilterUserZAByStudentSpecificId()
        {
            return _students.OrderByDescending(u => u.StudentSpecificId).ToList();
        }
    }
}
