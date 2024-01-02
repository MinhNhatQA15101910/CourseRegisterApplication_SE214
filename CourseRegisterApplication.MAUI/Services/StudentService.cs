using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class StudentService : IStudentService
{
    private readonly IServiceProvider _serviceProvider;

    public StudentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Student> AddStudent(Student student, List<PriorityType> priorityTypes)
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = GlobalConfig.STUDENT_BASE_URL;

        var json = JsonConvert.SerializeObject(student);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(new Uri(apiUrl), content);

        if (response.IsSuccessStatusCode)
        {
            // If student district is priority, add priority type with Id = 2.
            IDistrictService districtService = _serviceProvider.GetService<IDistrictService>();
            District district = await districtService.GetDistrictById(student.DistrictId);
            if (district.IsPriority)
            {
                IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
                List<PriorityType> priorityTypeList = (await priorityTypeService.GetAllPriorityTypesAsync()).ToList();
                PriorityType priorityType = priorityTypeList.First(pt => pt.Id == 2);

                priorityTypes.Add(priorityType);

                priorityType = priorityTypes.First(pt => pt.Id == 1);
                if (priorityType.Id == 1)
                {
                    priorityTypes.Remove(priorityType);
                }
            }

            // Add priority types
            IStudentPriorityTypeService studentPriorityTypeService = _serviceProvider.GetService<IStudentPriorityTypeService>();
            foreach (var priorityType in priorityTypes)
            {
                Student studentFromDatabase = await GetStudentBySpecificId(student.StudentSpecificId);

                StudentPriorityType studentPriorityType = new()
                {
                    PriorityTypeId = priorityType.Id,
                    StudentId = studentFromDatabase.Id
                };

                StudentPriorityType result = await studentPriorityTypeService.AddStudentPriorityType(studentPriorityType);
                if (result == null)
                {
                    return null;
                }
            }

            return student;
        }

        return null;
    }

    public async Task<List<Student>> GetAllStudents()
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = GlobalConfig.STUDENT_BASE_URL;

        var response = await httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
            return studentList;
        }

        return null;
    }

    public async Task<List<Student>> GetFullInformationOfAllStudents()
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}full/";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
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
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}full/specificId/{studentSpecificId}";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(jsonResponse);
        }

        return null;
    }

    public async Task<Student> GetStudentBySpecificId(string studentSpecificId)
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}specificId/{studentSpecificId}";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(jsonResponse);
        }

        return null;
    }

    public async Task<List<Student>> GetStudentsByBranchId(int branchId)
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}branch/{branchId}";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
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
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}district/{districtId}";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
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
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        student.ImageUrl = newImageUrl;

        string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}{student.Id}";

        var json = JsonConvert.SerializeObject(student);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync(new Uri(apiUrl), content);

        return response.IsSuccessStatusCode;
    }
}
