namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IDistrictService
    {
        Task<District> AddDistrict(District district);
        Task<bool> DeleteDistrict(int districtId);
        Task<List<District>> GetDistrictsByProvinceId(int provinceId);
    }
}
