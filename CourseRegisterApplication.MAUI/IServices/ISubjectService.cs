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
    }
}
