namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IProvinceService
    {
        Task<bool> DeleteProvince(int provinceId);
        Task<List<Province>> GetAllProvinces();
        Task<Province> GetProvinceById(int provinceId);
    }
}
