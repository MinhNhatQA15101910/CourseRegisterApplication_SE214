namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISemesterService
    {
        Task<List<Semester>> GetAllSemester();
        Task<Semester> GetSemesterByNameAndYear(string name, int year);
    }
}
