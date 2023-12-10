namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IProvinceService
    {
        Task<List<Province>> GetAllProvinces();
        Task<Province> GetProvinceById(int provinceId);
    }
}
