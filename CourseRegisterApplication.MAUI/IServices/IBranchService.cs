namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IBranchService
    {
        Task<Branch> AddBranch(Branch branch);
        Task<List<Branch>> GetAllBranches();
        Task<List<Branch>> GetBranchesByDepartmentId(int departmentId);
        Task<bool> UpdateBranch(int branchId, Branch branch);
    }
}
