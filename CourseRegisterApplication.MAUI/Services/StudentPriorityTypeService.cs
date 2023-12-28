using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class StudentPriorityTypeService : IStudentPriorityTypeService
{
    private readonly HttpClient _httpClient;

    public StudentPriorityTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StudentPriorityType> AddStudentPriorityType(StudentPriorityType studentPriorityType)
    {
        string apiUrl = GlobalConfig.STUDENT_PRIORITY_TYPE_BASE_URL;

        var json = JsonConvert.SerializeObject(studentPriorityType);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

        if (response.IsSuccessStatusCode)
        {
            return studentPriorityType;
        }

        return null;
    }

    public async Task<bool> DeleteStudentPriorityType(int studentId, int priorityTypeId)
    {
        string apiUrl = $"{GlobalConfig.STUDENT_PRIORITY_TYPE_BASE_URL}{studentId}/{priorityTypeId}";
        var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<StudentPriorityType>> GetStudentPriorityTypesByStudentId(int studentId)
    {
        string apiUrl = $"{GlobalConfig.STUDENT_PRIORITY_TYPE_BASE_URL}student/{studentId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var studentPriorityTypeList = JsonConvert.DeserializeObject<List<StudentPriorityType>>(jsonResponse);
            return studentPriorityTypeList;
        }

        return null;
    }
}
