using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class SubjectService : ISubjectService
    {
        public Task<Subject> AddSubject(Subject subject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSubject(int subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Subject>> GetAllSubjects()
        {
            throw new NotImplementedException();
        }

        public Task<Subject> GetSubjectById(int subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSubject(int subjectId, Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
