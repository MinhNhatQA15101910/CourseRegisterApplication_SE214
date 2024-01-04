using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class SemesterService : ISemesterService
{
    private readonly IServiceProvider _serviceProvider;

    public SemesterService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Semester> AddSemesterAsync(Semester semester, List<int> subjectIds)
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = GlobalConfig.SEMESTER_BASE_URL;

        var json = JsonConvert.SerializeObject(semester);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(new Uri(apiUrl), content);

        if (response.IsSuccessStatusCode)
        {
            IAvailableCourseService availableCourseService = _serviceProvider.GetService<IAvailableCourseService>();

            Semester createdSemester = await GetCurrentSemesterAsync();
            foreach (int subjectId in subjectIds)
            {
                AvailableCourse availableCourse = new()
                {
                    SemesterId = createdSemester.Id,
                    SubjectId = subjectId
                };

                AvailableCourse result = await availableCourseService.AddAvailableCourseAsync(availableCourse);
                if (result == null)
                {
                    return null;
                }
            }

            return semester;
        }

        return null;
    }

    public async Task<Semester> GetCurrentSemesterAsync()
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}current/";

        var response = await httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var currentSemester = JsonConvert.DeserializeObject<Semester>(jsonResponse);
            return currentSemester;
        }

        return null;
    }

    public async Task<bool> UpdateSemesterAsync(int semesterId, Semester semester)
    {
        HttpClient httpClient = _serviceProvider.GetService<HttpClient>();

        string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}{semesterId}";

        var json = JsonConvert.SerializeObject(semester);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync(new Uri(apiUrl), content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSemesterAsync(int semesterId, Semester semester, List<int> subjectIds)
    {
        bool result = await UpdateSemesterAsync(semesterId, semester);
        if (result)
        {
            // Delete old subjects
            IAvailableCourseService availableCourseService = _serviceProvider.GetService<IAvailableCourseService>();
            result = await availableCourseService.DeleteAvailableCoursesBySemesterIdAsync(semesterId);
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

                AvailableCourse result2 = await availableCourseService.AddAvailableCourseAsync(availableCourse);
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
