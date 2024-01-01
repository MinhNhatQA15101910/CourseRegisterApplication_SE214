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
        Task<List<Subject>> GetSubjectsByBranchIDSemester(int branchID, int semester);
        Task<List<Subject>> GetSubjectsByID(int subjectId);
        Task<List<Subject>> GetAllSubject();
        Task<Subject> GetSubjectById(int subjectId);
        Task<Subject> CreateSubject(Subject subject);
        Task<bool> DeleteSubject(int subjectId);
        Task<bool> UpdateSubject(int subjectId, Subject subject);
    }
}
