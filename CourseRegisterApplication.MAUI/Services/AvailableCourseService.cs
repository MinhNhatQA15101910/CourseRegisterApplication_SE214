using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class AvailableCourseService : IAvailableCourseService
{
    private readonly HttpClient _httpClient;

    public AvailableCourseService(IServiceProvider serviceProvider)
    {
        _httpClient = serviceProvider.GetService<HttpClient>();
    }

    public async Task<AvailableCourse> AddAvailableCourseAsync(AvailableCourse availableCourse)
    {
        string apiUrl = GlobalConfig.AVAILABLE_COURSE_BASE_URL;

        var json = JsonConvert.SerializeObject(availableCourse);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

        if (response.IsSuccessStatusCode)
        {
            return availableCourse;
        }

        return null;
    }

    public async Task<bool> DeleteAvailableCoursesBySemesterIdAsync(int semesterId)
    {
        string apiUrl = $"{GlobalConfig.SEMESTER_BASE_URL}semesterId/{semesterId}";
        var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

        return response.IsSuccessStatusCode;
    }
}
