namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICurriculumService
    {
        Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId);
    }
}
