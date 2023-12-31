namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectService
    {
       
        Task<Subject> AddSubject(Subject subject);
        
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(int subjectId);
        Task<Subject> CreateSubject(Subject subject);
        Task<bool> DeleteSubject(int subjectId);
        Task<bool> UpdateSubject(int subjectId, Subject subject);

    }
}
