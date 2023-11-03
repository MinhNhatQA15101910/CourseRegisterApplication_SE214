namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IRoleService
    {
        Task<Role> GetRole(int id);
    }
}
