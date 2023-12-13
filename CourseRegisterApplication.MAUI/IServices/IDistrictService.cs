namespace CourseRegisterApplication.MAUI.IServices
{
    interface IDistrictService
    {
        Task<List<District>> GetDistrictsByProvinceID(int provinceId);
    }
}
