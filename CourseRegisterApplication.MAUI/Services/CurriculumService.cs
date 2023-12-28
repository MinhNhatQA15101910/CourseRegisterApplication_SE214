using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services;

public class CurriculumService : ICurriculumService
{
    private readonly HttpClient _httpClient;

    public CurriculumService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId)
    {
        string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}branch/{branchId}";

        var response = await _httpClient.GetAsync(new Uri(apiUrl));
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var curriculumList = JsonConvert.DeserializeObject<List<Curriculum>>(jsonResponse);
            return curriculumList;
        }

        return null;
    }
}
