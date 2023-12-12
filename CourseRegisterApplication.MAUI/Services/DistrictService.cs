using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly HttpClient _httpClient;

        public DistrictService(HttpClient httpClient)
        {
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
    }
}
