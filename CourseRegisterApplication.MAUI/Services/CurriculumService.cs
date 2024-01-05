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
            string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}{branchId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var curriculumList = JsonConvert.DeserializeObject<List<Curriculum>>(jsonResponse);
                return curriculumList;
            }

            return null;
        }

        public async Task<List<Curriculum>> GetAllCurriculums()
        {
            string apiUrl = GlobalConfig.CURRICULUM_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var curriculumList = JsonConvert.DeserializeObject<List<Curriculum>>(jsonResponse);
                return curriculumList;
            }

            return null;
        }

        public async Task<List<Curriculum>> GetCurriculumsBySubjectId(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}BySubject/{subjectId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var curriculumList = JsonConvert.DeserializeObject<List<Curriculum>>(jsonResponse);
                return curriculumList;
            }

            return null;
        }

        public async Task<Curriculum> AddCurriculum(Curriculum curriculum)
        {
            string apiUrl = GlobalConfig.CURRICULUM_BASE_URL;

            var json = JsonConvert.SerializeObject(curriculum);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return curriculum;
            }
            return null;
        }

        public async Task<List<Curriculum>> GetCurriculumSubjectsByBranchIdAndSemester(int branchId, int semester)
        {
            string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}branch/{branchId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var curriculumList = JsonConvert.DeserializeObject<List<Curriculum>>(jsonResponse);

                var semeterSubjectList = curriculumList.Where(c => c.Semester == semester).ToList();
                return semeterSubjectList;
            }

            return null;
        }

        public async Task<bool> DeleteCurriculum(int branchId, int subjectId)
        {
            string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}{branchId}/{subjectId}";

            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
