using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class PriorityTypeService : IPriorityTypeService
{
    private readonly HttpClient _httpClient;

    public PriorityTypeService(IServiceProvider serviceProvider)
    {
        _httpClient = serviceProvider.GetService<HttpClient>();
    }

    public async Task<IEnumerable<PriorityType>> GetAllPriorityTypesAsync()
    {
        string apiUrl = GlobalConfig.PRIORITY_TYPE_BASE_URL;

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var priorityTypeList = JsonConvert.DeserializeObject<List<PriorityType>>(jsonResponse);
            return priorityTypeList;
        }

        return null;
    }

    public async Task<IEnumerable<PriorityType>> GetPriorityTypesFromStudentIdAsync(int studentId)
    {
        string apiUrl = $"{GlobalConfig.PRIORITY_TYPE_BASE_URL}studentId/{studentId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var priorityTypeList = JsonConvert.DeserializeObject<List<PriorityType>>(jsonResponse);
            return priorityTypeList;
        }

        return null;
    }
}
