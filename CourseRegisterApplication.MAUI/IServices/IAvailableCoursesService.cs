namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAvailableCoursesService
    {
        Task<List<AvailableCourse>> GetAvailableCourses();
        Task<List<AvailableCourse>> GetAvailableCoursesBySubjectId(int subjectId);
    }
}
