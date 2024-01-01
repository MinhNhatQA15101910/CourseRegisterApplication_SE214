namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationDetailService
    {
        Task<List<CourseRegistrationDetail>> GetAllCRD();
        Task<CourseRegistrationDetail> GetCRDByCRFId(int crfId);
        Task<List<CourseRegistrationDetail>> GetCourseRegistrationDetailBySubjectId(int subjectId);
        Task<CourseRegistrationDetail> CreateCourseRegistrationDetail(CourseRegistrationDetail courseRegistrationDetail);
        Task<bool> DeleteCourseRegistrationDetail(int courseRegistrationFormId, int subjectId);
    }
}
