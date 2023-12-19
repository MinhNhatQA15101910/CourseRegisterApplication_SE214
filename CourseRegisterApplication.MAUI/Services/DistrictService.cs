using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;

        public DistrictService(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
        }

        public async Task<District> AddDistrict(District district)
        {
            string apiUrl = GlobalConfig.DISTRICT_BASE_URL;

            var json = JsonConvert.SerializeObject(district);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return district;
            }

            return null;
        }

        public async Task<bool> DeleteDistrict(int districtId)
        {
            string apiUrl = $"{GlobalConfig.DISTRICT_BASE_URL}{districtId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<District>> GetDistrictsByProvinceId(int provinceId)
        {
            string apiUrl = $"{GlobalConfig.DISTRICT_BASE_URL}province/{provinceId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var districtList = JsonConvert.DeserializeObject<List<District>>(jsonResponse);
                return districtList;
            }

            return null;
        }

        public async Task<bool> UpdateDistrict(int districtId, District district)
        {
            string apiUrl = $"{GlobalConfig.DISTRICT_BASE_URL}{districtId}";

            var json = JsonConvert.SerializeObject(district);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                // Update priority types of students belong to the updated district
                var studentService = _serviceProvider.GetService<IStudentService>();
                var studentPriorityTypeService = _serviceProvider.GetService<IStudentPriorityTypeService>();

                var students = await studentService.GetStudentsByDistrictId(districtId);
                if (students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        if (district.IsPriority)
                        {
                            var studentPriorityType = await studentPriorityTypeService.AddStudentPriorityType(new() { StudentId = student.Id, PriorityTypeId = 2 });
                            if (studentPriorityType == null)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            var success = await studentPriorityTypeService.DeleteStudentPriorityType(student.Id, 2);
                            if (!success)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
