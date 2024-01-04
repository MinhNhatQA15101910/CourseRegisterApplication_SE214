using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class SemesterService : ISemesterService
{
    private readonly HttpClient _httpClient;

    public SemesterService(IServiceProvider serviceProvider)
    {
        _httpClient = serviceProvider.GetService<HttpClient>();
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
}
