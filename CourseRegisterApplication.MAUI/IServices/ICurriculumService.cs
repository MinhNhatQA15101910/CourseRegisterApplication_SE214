namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICurriculumService
    {
        Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId);
        Task<List<Curriculum>> GetCurriculumsBySubjectId(int branchId);
    }
}
