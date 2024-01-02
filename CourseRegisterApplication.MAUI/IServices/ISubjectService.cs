namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetAllSubject();
        Task<Subject> GetSubjectById(int subjectId);
        Task<Subject> CreateSubject(Subject subject);
        Task<bool> DeleteSubject(int subjectId);
        Task<bool> UpdateSubject(int subjectId, Subject subject);

    }
}
