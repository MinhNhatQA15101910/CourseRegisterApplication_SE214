using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly HttpClient _httpClient;

        public SubjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Subject> GetSubjectById(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subject = JsonConvert.DeserializeObject<Subject>(jsonResponse);
                return subject;
            }

            return null;
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var subjects = JsonConvert.DeserializeObject<List<Subject>>(jsonResponse);
                return subjects;
            }

            return null;
        }

        public async Task<Subject> CreateSubject(Subject subject)
        {
            string apiUrl = GlobalConfig.SUBJECT_BASE_URL;

            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return subject;
            }

            return null;
        }

        public async Task<bool> DeleteSubject(int subjectId)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateSubject(int subjectId, Subject subject)
        {
            string apiUrl = $"{GlobalConfig.SUBJECT_BASE_URL}{subjectId}";

            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

    }
}
