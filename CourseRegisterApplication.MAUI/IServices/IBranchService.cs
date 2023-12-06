namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IBranchService
    {
        Task<List<Branch>> GetBranchesByDepartmentId(int departmentId);
    }
}
