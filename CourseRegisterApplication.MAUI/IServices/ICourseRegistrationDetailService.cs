namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationDetailService
    {
        Task<CourseRegistrationDetail> CreateCourseRegistrationDetail(CourseRegistrationDetail courseRegistrationDetail);

        Task<bool> DeleteCourseRegistrationDetail(int crdId);

        Task<CourseRegistrationDetail> GetCRDByCRFId (int crfId);

        Task<List<CourseRegistrationDetail>> GetCourseRegistrationDetailBySubjectId(int subjectId);
    }
}
