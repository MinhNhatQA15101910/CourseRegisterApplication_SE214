namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICurriculumService
    {
        Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId);

        Task<List<Curriculum>> GetCurriculumSubjectsByBranchIdAndSemester(int branchId, int semester);
        Task<List<Curriculum>> GetCurriculumsBySubjectId(int branchId);
    }
}
