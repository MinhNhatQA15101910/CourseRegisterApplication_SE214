namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IProvinceService
    {
        Task<Province> AddProvince(Province province);
        Task<bool> DeleteProvince(int provinceId);
        Task<List<Province>> GetAllProvinces();
        Task<Province> GetProvinceById(int provinceId);
        Task<bool> UpdateProvince(int provinceId, Province province);
    }
}
