namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<IEnumerable<Subject>> GetSubjectsForFirstSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsForSecondSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsForSummerSemesterAsync();
        Task<IEnumerable<Subject>> GetSubjectsBySemesterIdAsync(int semesterId);
    }
}
