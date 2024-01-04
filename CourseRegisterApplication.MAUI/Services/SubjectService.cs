using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class SubjectService : ISubjectService
{
    private readonly HttpClient _httpClient;

    public SubjectService(IServiceProvider serviceProvider)
    {
        _httpClient = serviceProvider.GetService<HttpClient>();
    }

    public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
    {
        string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
            return subjectList;
        }

        return null;
    }

    public async Task<IEnumerable<Subject>> GetSubjectsBySemesterIdAsync(int semesterId)
    {
        string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}semesterId/{semesterId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
            return subjectList;
        }

        return null;
    }

    public async Task<IEnumerable<Subject>> GetSubjectsForFirstSemesterAsync()
    {
        string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}firstSemester/";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
            return subjectList;
        }

        return null;
    }

    public async Task<IEnumerable<Subject>> GetSubjectsForSecondSemesterAsync()
    {
        string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}secondSemester/";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
            return subjectList;
        }

        return null;
    }

    public async Task<IEnumerable<Subject>> GetSubjectsForSummerSemesterAsync()
    {
        string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}summerSemester/";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
            return subjectList;
        }

        return null;
    }
}
