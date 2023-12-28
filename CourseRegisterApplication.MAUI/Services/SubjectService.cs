using CourseRegisterApplication.MAUI.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.Services
{
    class SubjectService : ISubjectService
    {
        private readonly HttpClient _httpClient;

        public SubjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Subject>> GetAllSubjects()
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
        public async Task<List<Subject>> GetSubjectsByBranchIDSemester(int branchID, int semester)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}curriculum/{branchID}/{semester}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjectList = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjectList;
            }

            return null;
        }

        public async Task<List<Subject>> GetSubjectsByID(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}/{subjectId}";

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
}
