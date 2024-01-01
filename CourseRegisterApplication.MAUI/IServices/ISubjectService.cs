using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectService
    {
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(int subjectId);
        Task<Subject> CreateSubject(Subject subject);
        Task<bool> DeleteSubject(int subjectId);
        Task<bool> UpdateSubject(int subjectId, Subject subject);
    }
}
