namespace CourseRegisterApplication.MAUI.IServices
{
    interface IProvinceService
    {
        Task<List<Province>> GetAllProvince();
    }
}
