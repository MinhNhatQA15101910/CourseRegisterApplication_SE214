using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class CourseRegistrationDetailService : ICourseRegistrationDetailService
    {
        public Task<List<CourseRegistrationDetail>> GetAllCourseRegistrationDetail()
        {
            throw new NotImplementedException();
        }

        public Task<List<CourseRegistrationDetail>> GetCourseRegistrationDetailBySubjectId(int subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
