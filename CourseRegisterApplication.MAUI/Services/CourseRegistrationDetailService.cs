using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;
using System.Net.Http;

namespace CourseRegisterApplication.MAUI.Services
{
    public class CourseRegistrationDetailService : ICourseRegistrationDetailService
    {
        private readonly HttpClient _httpClient;

        public CourseRegistrationDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseRegistrationDetail>> GetAllCRD()
        {
            string apiUrl = GlobalConfig.COURSE_REGISTRATION_DETAIL_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationDetails = JsonConvert.DeserializeObject<List<CourseRegistrationDetail>>(jsonResponse);
                return courseRegistrationDetails;
            }

            return null;
        }

        public async Task<CourseRegistrationDetail> CreateCourseRegistrationDetail(CourseRegistrationDetail courseRegistrationDetail)
        {
            string apiUrl = GlobalConfig.COURSE_REGISTRATION_DETAIL_BASE_URL;

            var json = JsonConvert.SerializeObject(courseRegistrationDetail);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return courseRegistrationDetail;
            }

            return null;
        }
        public async Task<bool> DeleteCourseRegistrationDetail(int courseRegistrationFormId, int subjectId)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_DETAIL_BASE_URL}{courseRegistrationFormId}{subjectId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<CourseRegistrationDetail> GetCRDByCRFId(int crfId)
        {
            string apiUrl = $"{GlobalConfig.COURSE_REGISTRATION_DETAIL_BASE_URL}{crfId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CourseRegistrationDetail>(jsonResponse);
            }

            return null;
        }

        public async Task<List<CourseRegistrationDetail>> GetCourseRegistrationDetailBySubjectId(int subjectId)
        {
            string apiUrl = GlobalConfig.COURSE_REGISTRATION_DETAIL_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var courseRegistrationDetails = JsonConvert.DeserializeObject<List<CourseRegistrationDetail>>(jsonResponse);
                return courseRegistrationDetails.Where(crd => crd.SubjectId == subjectId).ToList();
            }

            return null;
        }
    }
}
