namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IBranchService
    {
        Task<Branch> AddBranch(Branch branch);
        Task<bool> DeleteBranch(int branchId);
        Task<List<Branch>> GetAllBranches();
        Task<Branch> GetBranchById(int branchId);
        Task<List<Branch>> GetBranchesByDepartmentId(int departmentId);
        Task<bool> UpdateBranch(int branchId, Branch branch);
    }
}
