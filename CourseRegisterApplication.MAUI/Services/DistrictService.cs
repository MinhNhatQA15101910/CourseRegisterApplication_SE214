using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly HttpClient _httpClient;

        public DistrictService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
