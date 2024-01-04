namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISemesterService
    {
        Task<Semester> GetCurrentSemesterAsync();
    }
}
