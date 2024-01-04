namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISemesterService
    {
        Task<Semester> AddSemesterAsync(Semester semester, List<int> subjectIds);
        Task<Semester> GetCurrentSemesterAsync();
        Task<bool> UpdateSemesterAsync(int semesterId, Semester semester);
        Task<bool> UpdateSemesterAsync(int semesterId, Semester semester, List<int> subjectIds);
    }
}
