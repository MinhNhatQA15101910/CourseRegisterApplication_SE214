namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IDistrictService
    {
        Task<District> AddDistrict(District district);
        Task<bool> DeleteDistrict(int districtId);
        Task<District> GetDistrictById(int districtId);
        Task<List<District>> GetDistrictsByProvinceId(int provinceId);
        Task<District> GetDistrictById(int districtId);
        Task<bool> UpdateDistrict(int districtId, District district);
    }
}
