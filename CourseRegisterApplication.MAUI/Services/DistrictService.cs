using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class DistrictService : IDistrictService
    {
        public Task<List<District>> GetDistrictsByProvinceID(int provinceId)
        {
            throw new NotImplementedException();
        }
    }
}
