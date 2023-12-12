namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IDistrictService
    {
        Task<bool> DeleteDistrict(int districtId);
        Task<List<District>> GetDistrictsByProvinceId(int provinceId);
    }
}
