﻿using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly HttpClient _httpClient;

        public CurriculumService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId)
        {
            string apiUrl = $"{GlobalConfig.CURRICULUM_BASE_URL}ByBranch/{branchId}";

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

        public async Task<List<Curriculum>> GetCurriculumsBySubjectId(int subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
