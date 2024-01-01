using CourseRegisterApplication.MAUI.IServices;

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
    }
}
