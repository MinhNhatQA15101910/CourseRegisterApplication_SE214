namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationFormService
    {
        Task<CourseRegistrationForm> CreateCourseRegistrationForm(CourseRegistrationForm courseRegistrationForm);
        Task<List<CourseRegistrationForm>> GetCourseRegistrationFormByStudentId(int studentId);
        Task<CourseRegistrationForm> GetCourseRegistrationFormByStudentIdAndSemesterId(int studentId, int semesterId);
    }
}
