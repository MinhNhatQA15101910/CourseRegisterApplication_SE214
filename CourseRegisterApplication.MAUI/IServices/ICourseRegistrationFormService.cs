namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationFormService
    {
        Task<List<CourseRegistrationForm>> GetAllCourseRegistrationForm();
        Task<CourseRegistrationForm> CreateCourseRegistrationForm(CourseRegistrationForm courseRegistrationForm);
        Task<List<CourseRegistrationForm>> GetCourseRegistrationFormByStudentId(int studentId);
        Task<CourseRegistrationForm> GetCourseRegistrationFormByStudentIdAndSemesterId(int studentId, int semesterId);
    }
}
