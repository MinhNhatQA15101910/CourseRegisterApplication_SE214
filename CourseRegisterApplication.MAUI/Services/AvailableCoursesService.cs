using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class AvailableCoursesService : IAvailableCoursesService
    {
        public Task<List<AvailableCourse>> GetAvailableCourses()
        {
            throw new NotImplementedException();
        }

        public Task<List<AvailableCourse>> GetAvailableCoursesBySubjectId(int subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
