using AuthenticationServices;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.Services
{
    public class TuitionFeeReceiptService : ITuitionFeeReceiptService
    {
        private readonly HttpClient _httpClient;

        public TuitionFeeReceiptService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TuitionFeeReceipt>> GetAllTuitionFeeReceipt()
        {
            string apiUrl = $"{GlobalConfig.TUITION_FEE_RECEIPT_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var tuitionFeeReceipts = JsonConvert.DeserializeObject<List<TuitionFeeReceipt>>(jsonResponse);
                return tuitionFeeReceipts;
            }

            return null;
        }
        public async Task<List<TuitionFeeReceipt>> GetTuitionFeeReceiptsByCourseRegistrationFormId(int courseRegistrationFormId)
        {
            string apiUrl = $"{GlobalConfig.TUITION_FEE_RECEIPT_BASE_URL}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var tuitionFeeReceipts = JsonConvert.DeserializeObject<List<TuitionFeeReceipt>>(jsonResponse);
                return tuitionFeeReceipts.Where(tFR => tFR.CourseRegistrationFormId == courseRegistrationFormId).ToList();
            }

            return null;
        }

        public async Task<TuitionFeeReceipt> CreateTuitionFeeReceipt(TuitionFeeReceipt tuitionFeeReceipt)
        {
            string apiUrl = GlobalConfig.TUITION_FEE_RECEIPT_BASE_URL;

            var json = JsonConvert.SerializeObject(tuitionFeeReceipt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                return tuitionFeeReceipt;
            }

            return null;
        }
        public async Task<bool> UpdateTuitionFeeReceipt(int id, TuitionFeeReceipt tuitionFeeReceipt)
        {
            string apiUrl = $"{GlobalConfig.TUITION_FEE_RECEIPT_BASE_URL}{id}";

            var json = JsonConvert.SerializeObject(tuitionFeeReceipt);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }
    }
}
