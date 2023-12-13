using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly HttpClient _httpClient;

        public ProvinceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Province> AddProvince(Province province)
        {
            string apiUrl = GlobalConfig.PROVINCE_BASE_URL;

            var json = JsonConvert.SerializeObject(province);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return province;
            }

            return null;
        }

        public async Task<bool> DeleteProvince(int provinceId)
        {
            string apiUrl = $"{GlobalConfig.PROVINCE_BASE_URL}{provinceId}";
            var response = await _httpClient.DeleteAsync(new Uri(apiUrl)).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Province>> GetAllProvinces()
        {
            string apiUrl = GlobalConfig.PROVINCE_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var provinceList = JsonConvert.DeserializeObject<List<Province>>(jsonResponse);
                return provinceList;
            }

            return null;
        }

        public async Task<Province> GetProvinceById(int provinceId)
        {
            string apiUrl = $"{GlobalConfig.PROVINCE_BASE_URL}{provinceId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Province>(jsonResponse);
            }

            return null;
        }

        public async Task<bool> UpdateProvince(int provinceId, Province province)
        {
            string apiUrl = $"{GlobalConfig.PROVINCE_BASE_URL}{provinceId}";

            var json = JsonConvert.SerializeObject(province);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }
    }
}
