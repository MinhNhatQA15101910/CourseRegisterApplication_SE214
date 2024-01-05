using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<IEnumerable<Subject>> GetSubjectsForFirstSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsForSecondSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsForSummerSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsBySemesterIdAsync(int semesterId);
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectById(int subjectId);
        Task<Subject> CreateSubject(Subject subject);
        Task<bool> DeleteSubject(int subjectId);
        Task<bool> UpdateSubject(int subjectId, Subject subject);

    }
}
