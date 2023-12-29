using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

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

    public async Task<Student> GetFullInformationOfStudentBySpecificId(string studentSpecificId)
    {
        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}full/specificId/{studentSpecificId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(jsonResponse);
        }

        return null;
    }

    public async Task<Student> GetStudentBySpecificId(string studentSpecificId)
    {
        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}specificId/{studentSpecificId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(jsonResponse);
        }

        return null;
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

    public async Task<bool> UpdateImageUrl(Student student, string newImageUrl)
    {
        student.ImageUrl = newImageUrl;

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}{student.Id}";

        var json = JsonConvert.SerializeObject(student);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

        return response.IsSuccessStatusCode;
    }
}
