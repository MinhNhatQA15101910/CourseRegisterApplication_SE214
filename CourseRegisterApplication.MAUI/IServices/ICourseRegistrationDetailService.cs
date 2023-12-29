namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationDetailService
    {
        Task<List<CourseRegistrationDetail>> GetAllCourseRegistrationDetail();

        Task<List<CourseRegistrationDetail>> GetCourseRegistrationDetailBySubjectId(int subjectId);
    }
}
