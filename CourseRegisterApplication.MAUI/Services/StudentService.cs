using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;
using Newtonsoft.Json;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
        private readonly string _baseUrl = "https://localhost:7182/api/Students/";
        private readonly string _branchUrl = "https://localhost:7182/api/Branches/";
        private readonly HttpClient _httpClient = new HttpClient();
        private List<Student> _students;
        private List<Branch> _branches;

        public async Task<List<Student>> GetAllStudent()
        {
            try
            {
                string studentsApiUrl = $"{_baseUrl}";

                using (_httpClient)
                {
                    var studentsResponse = await _httpClient.GetAsync(new Uri(studentsApiUrl)).ConfigureAwait(false);

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
        public async Task<List<Branch>> GetAllBranches()
        {
            try
            {
                string branchesApiUrl = $"{_branchUrl}";

                using (_httpClient)
                {
                    var branchesResponse = await _httpClient.GetAsync(new Uri(branchesApiUrl)).ConfigureAwait(false);

                    if (branchesResponse.IsSuccessStatusCode)
                    {
                        string branchJsonResponse = await branchesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var branches = JsonConvert.DeserializeObject<List<Branch>>(branchJsonResponse);

                        _branches = branches.ToList();
                        return _branches;
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
            return _students
                .Where(u => u.StudentSpecificId.Contains(filter) || _branches.Any(b => b.Id == u.BranchId && b.BranchName.Contains(filter)))
                .ToList();
        }
        public List<Student> FilterStudentAZByStudentSpecificId()
        {
            return _students.OrderBy(u => u.StudentSpecificId).ToList();
        }
        public List<Student> FilterStudentZAByStudentSpecificId()
        {
            return _students.OrderByDescending(u => u.StudentSpecificId).ToList();
        }
    }
}
