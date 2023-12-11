namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IDistrictService
    {
        Task<List<District>> GetDistrictsByProvinceId(int provinceId);
    }
}
