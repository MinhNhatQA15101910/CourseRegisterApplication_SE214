namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICurriculumService
    {
        Task<List<Curriculum>> GetCurriculumsByBranchId(int branchId);
        Task<List<Curriculum>> GetAllCurriculums();
        Task<Curriculum> AddCurriculum(Curriculum curriculum);
        Task<bool> DeleteCurriculum(int branchId, int subjectId);

        Task<List<Curriculum>> GetCurriculumsBySubjectId(int subjectId);
        Task<List<Curriculum>> GetCurriculumSubjectsByBranchIdAndSemester(int branchId, int semester);
    }
}
