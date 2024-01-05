namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationFormService
    {
        Task<List<CourseRegistrationForm>> GetAllCourseRegistrationFormsWithPendingState();
        Task<CourseRegistrationForm> GetCourseRegistrationFormById(int id);
        Task<CourseRegistrationForm> CreateCourseRegistrationForm(CourseRegistrationForm courseRegistrationForm);
        Task<bool> UpdateCourseRegistrationForm(int id, CourseRegistrationForm courseRegistrationForm);
        Task<List<CourseRegistrationForm>> GetCourseRegistrationFormByStudentId(int studentId);
        Task<CourseRegistrationForm> GetCourseRegistrationFormByStudentIdAndSemesterId(int studentId, int semesterId);
    }
}