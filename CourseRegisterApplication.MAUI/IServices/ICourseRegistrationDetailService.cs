namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ICourseRegistrationDetailService
    {
        Task<CourseRegistrationDetail> CreateCourseRegistrationDetail(Branch branch);
        Task<bool> DeleteCourseRegistrationDetail(int crdId);
        Task<CourseRegistrationDetail> GetCRDByCRFId (int crfId);
    }
}
